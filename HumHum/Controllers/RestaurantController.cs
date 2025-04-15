using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;

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

        //CustomerCartDto Count;
        //try
        //{
        //    Count = await _serviceManager.CartService.GetCustomerCartAsync("54150542-7e35-4fe3-9fdc-ee0a383d4f07");

        //    HttpContext.Session.SetInt32("CartCount", Count.Items.Count);
        //}
        //catch
        //{
        //    HttpContext.Session.SetInt32("CartCount", 0);
        //}

        return View(products);
    }
}