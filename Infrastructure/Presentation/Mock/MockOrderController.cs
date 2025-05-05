using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.OrderModule;

namespace Presentation.Mock;

public class MockOrderController : Controller
{

    private readonly IServiceManager _serviceManager;

    public MockOrderController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }


    public IActionResult Create() => View();


    [HttpPost]
    public async Task<IActionResult> Create(OrderToCreationViewModel model)
    {
        var res = await _serviceManager.OrderService.CreateOrderAsync(model, "customer@gmail.com");

        return RedirectToAction(nameof(ShowOrder), new { id = res.Id });

    }


    [HttpGet]
    public async Task<IActionResult> ShowOrder(Guid id)
    {

        var res = await _serviceManager.OrderService.GetOrderForUserByIdAsync(id);



        return View(res);

    }










}
