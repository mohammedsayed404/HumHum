using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Service.Abstractions;
using Shared.ViewModels;

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

    public async Task<IActionResult> Details(int? id, string viewName = nameof(Details))
    {
        if (!id.HasValue) return BadRequest();

        var restaurant = await _serviceManager.RestaurantService.GetRestaurantByIdAsync(id.Value);

        if (restaurant is null) return NotFound();

        return View(viewName, restaurant);

    }

    public async Task<IActionResult> ProductsToResturant(int id)
    {
        //var restaurants = await _serviceManager.RestaurantService.GetAllRestaurantsAsync();

        var products = await _serviceManager.RestaurantService.GetAllProductsOfRestorantById(id);


        return View(products);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(RestaurantToCreationViewModel model)
    {
       
        if (!ModelState.IsValid) return View(model);

        var created = await _serviceManager.RestaurantService.CreateRestaurantAsync(model);

        if (created > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "Unable to create restaurant");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id,
        [FromServices] IUnitOfWork _unitOfWork, [FromServices] IMapper _mapper)
    {
        if (!id.HasValue) return BadRequest();

        var restaurant = await _unitOfWork.GetRepository<Restaurant, int>().GetByIdAsync(id.Value);

        if (restaurant is null) return NotFound();


        var mappedRestaurant = _mapper.Map<RestaurantToUpdateViewModel>(restaurant);


        return View(mappedRestaurant);

    }


    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, RestaurantToUpdateViewModel model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid) return View(model);

        var updated = await _serviceManager.RestaurantService.UpdateRestaurantAsync(model);

        if (updated > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "can't Update restaurant pls try again later");
            return View(model);
        }
    }




    [HttpGet]
    public async Task<IActionResult> Delete(int? id) => await Details(id, nameof(Delete));



    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted = await _serviceManager.RestaurantService.DeleteRestaurantAsync(id);

        if (deleted > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "can't delete restaurant pls try again later");

            return await Details(id, nameof(Delete));
        }
    }
}
