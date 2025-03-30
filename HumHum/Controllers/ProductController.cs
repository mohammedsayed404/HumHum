using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace HumHum.Controllers;

public class ProductController : Controller
{
    private readonly IServiceManager _serviceManager;

    public ProductController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _serviceManager.ProductService.GetAllProductsAsync();

        return View(products);
    }


    [HttpGet]
    public IActionResult Create() => View();




}
