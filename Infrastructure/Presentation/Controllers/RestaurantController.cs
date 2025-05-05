using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;
using Shared.ViewModels;

namespace Presentation.Controllers;

[Authorize]
public class RestaurantController : Controller
{

    private readonly IServiceManager _serviceManager;

    private readonly string cartId;
    public RestaurantController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
        cartId = _serviceManager.UserServices.Id!;
    }

    public async Task<IActionResult> Index()
    {
        //var restaurants = await _serviceManager.ProductService.GetAllRestaurantsAsync();
        var restaurants = await _serviceManager.RestaurantService.GetAllRestaurantsAsync();

        //TempData["restaurantNames"] = restaurants.Select(r => r.Name); 

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

        try
        {
            var products = await _serviceManager.RestaurantService.GetAllProductsOfRestorantById(id);

            var customerCart =
                await _serviceManager.CartService.GetCustomerCartAsync(cartId);

            var items = customerCart.Items;


            var productsWithQuantity = new ProductToRestaurantWithQuantityViewModel()
            {
                Products = products.ToList(),
                RestaurantName = products[0].Restaurant,
                Quantity = Enumerable.Repeat(0, products.Count).ToList(),
                RestaurantId = id
            };

            if (items.Count != 0)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    productsWithQuantity.Quantity[i] =
                        items.FirstOrDefault(item => item.Id == products[i].Id)?.Quantity ?? 0;
                }
            }


            //var restaurant = await _serviceManager.RestaurantService.GetRestaurantByIdAsync(id);

            //var restaruantsNames = TempData["restaurantNames"] as IReadOnlyList<string>;

            //var restaurantName = restaruantsNames.

            //ViewBag.restaurantName = products[0].Restaurant;

            //ViewBag.customerProducts = items;

            return View(productsWithQuantity);

        }
        catch
        {
            return RedirectToAction("Index", "Restaurant");
        }
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

