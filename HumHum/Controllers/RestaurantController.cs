using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;

namespace HumHum.Controllers;

public class RestaurantController : Controller
{

    private readonly IServiceManager _serviceManager;

    public RestaurantController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task<IActionResult> Index()
    {
        //var restaurants = await _serviceManager.ProductService.GetAllRestaurantsAsync();
        var restaurants = await _serviceManager.RestaurantService.GetAllRestaurantsAsync();

        return View(restaurants);
    }

    public async Task<IActionResult> ProductsToResturant(int id)
    {
        //var restaurants = await _serviceManager.RestaurantService.GetAllRestaurantsAsync();

        var products = await _serviceManager.RestaurantService.GetAllProductsOfRestorantById(id);


        return View(products);
    }
}
