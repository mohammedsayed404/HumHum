using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace HumHum.Controllers;

public class ProductCategoryController : Controller
{

    private readonly IServiceManager _serviceManager;

    public ProductCategoryController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _serviceManager.ProductService.GetAllCategoriesAsync();

        return View(categories);
    }

}
