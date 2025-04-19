using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Stripe;

namespace HumHum.Controllers;

public class PaymentController : Controller
{
    private readonly IServiceManager _ServiceManger;

    public PaymentController(IServiceManager serviceManger)
    {
        _ServiceManger = serviceManger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdatePaymentIntent(string cardId)
    {
        if (cardId == null) return BadRequest();
        var card = await _ServiceManger.PaymentService.CreateOrUpdatePaymentIntent(cardId);

        if (cardId is null) return BadRequest();

        return View(card);
    }

    const string endpointSecret = "whsec_...";

    [HttpPost]
    public async  Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //const string endpointSecret = "whsec_...";
        
            
        var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

        // If on SDK version < 46, use class Events instead of EventTypes
        if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
        {
            _ServiceManger.PaymentService.UpdatePaymentIntentForSucceededOrFailed(paymentIntent.Id, false);
        }
        else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
        {
            _ServiceManger.PaymentService.UpdatePaymentIntentForSucceededOrFailed(paymentIntent.Id, true);

        }
        else
        {
            Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        }
        return View();
        
      
    }
}
