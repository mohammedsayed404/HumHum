using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace HumHum.Controllers;

public class MockCartController : Controller
{
    private readonly IServiceManager _serviceManager;

    public MockCartController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    ///          ICartService       ,  IOrderService
    //[min , plus , index , Delete ],   checkout  ,




}
