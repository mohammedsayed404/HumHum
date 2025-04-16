using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace HumHum.Controllers;

public class PaymentController : Controller
{
    private readonly IServiceManager _ServiceManger;

    public PaymentController(IServiceManager serviceManger)
    {
        _ServiceManger = serviceManger;
    }


    //[HttpPost("cardId")]

    //why generic actionResult  ==>  for  api  or  all  ?

    #region Until Finshing Payment
    //[HttpPost("cardId")]

    ////why generic actionResult  ==>  for  api  or  all  ?

    //public async Task<ActionResult> CreateOrUpdatePaymentIntent(string cardId)
    //{
    //    var card = await _ServiceManger.PaymentService.CreateOrUpdatePaymentIntent(cardId);

    //    if (cardId is null) return BadRequest();

    //    return View(card);

    //}

    //} 
    #endregion

}
