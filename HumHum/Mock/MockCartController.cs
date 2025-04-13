using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;

namespace HumHum.Mock;


public class MockCartController : Controller
{
    private readonly IServiceManager _serviceManager;

    public MockCartController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    ///          ICartService       ,  IOrderService[Auroize]
    //[min , plus , index , Delete ],   Checkout => Order Controller => Order Service ,

    ///Shop Controller  => Products , Restaurant  , Category , 
    /////Cart Shoping => 




    [HttpGet]
    public IActionResult Create() => View();


    [HttpPost]
    public IActionResult Create(CustomerCartDto cart)
    {

        var res = _serviceManager.CartService.UpdateCustomerCartAsync(cart);



        return RedirectToAction(nameof(ShowCart), new { id = cart.Id });

    }

    [HttpGet]
    public async Task<IActionResult> ShowCart(string id)
    {

        var res = await _serviceManager.CartService.GetCustomerCartAsync(id);



        return View(res);

    }

}
