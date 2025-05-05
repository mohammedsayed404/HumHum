using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Security.Claims;

namespace Presentation.Mock;

public class MockCheckoutController : Controller
{
    private readonly IServiceManager _serviceManager;

    public MockCheckoutController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }


    //get address 




    void testc()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);


        //_serviceManager.UserServices.Id => cart id 


        //_serviceManager.UserServices.UserEmail

        //_serviceManager.OrderService.CreateOrderAsync(
        //    ,_serviceManager.UserServices.UserEmail
        //    )

    }

    ///get addredd , get delivery id ,
    /// 
}
