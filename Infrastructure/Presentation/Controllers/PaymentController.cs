using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;
using Shared.OrderModule;
using Stripe;

namespace Presentation.Controllers;

public class PaymentController : Controller
{
    private readonly IServiceManager _ServiceManger;

    public PaymentController(IServiceManager serviceManger)
    {
        _ServiceManger = serviceManger;
    }

    // get from salah checkout
    [HttpPost]
    public async Task<IActionResult> Details(DeliveryMethodToReturnDto deliveryMethodToReturnDto)
    {
        //public record OrderToReturnDto(Guid Id, string UserEmail,
        //string PaymentStatus, string DeliveryMethod, decimal Subtotal, decimal Total,
        //string PaymentIntentId, DateTimeOffset OrderDate, OrderAddressToReturnDto ShippingAddress,
        //IReadOnlyList<OrderItemToReturnDto> OrderItems
        //);

        //var order = await _ServiceManger.OrderService.GetOrderForUserByIdAsync(orderId);
        //_ServiceManger.UserServices.


        //var OverAllOrder = new OrderToReturnDto()

        ///create  order  -orderser  cartId, address, email,demthod
        ///demethod from checkout controller  
        ///addreess from  user  address  if  changed  form order to create  new  model
        ///
        ///
        ///
        ///


        return View();
    }


    //[HttpPost]
    public async Task<IActionResult> CreateOrUpdatePaymentIntent(OrderToCreationViewModel model)
    {
        string cartId = _ServiceManger.UserServices.Id;
        if (cartId == null) return BadRequest();
        var cart = await _ServiceManger.PaymentService.CreateOrUpdatePaymentIntent(cartId);

        var customerCart = new CustomerCartDto(cartId, cart.Items, cart.DeliveryMethodId, cart.DeliveryPrice, cart.PaymentIntentId, cart.ClientSecret);
        if (customerCart is null) return BadRequest();

        //return Json(new { success = true, message = "Product is increased by one" });
        return View(customerCart);
    }



    const string endpointSecret = "whsec_...";

    [HttpPost]
    public async Task<IActionResult> Index()
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
