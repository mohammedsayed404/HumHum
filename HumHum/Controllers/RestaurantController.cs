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
        var restaurants = await _serviceManager.ProductService.GetAllRestaurantsAsync();

        return View(restaurants);
    }


    //Create , Delete 




}
