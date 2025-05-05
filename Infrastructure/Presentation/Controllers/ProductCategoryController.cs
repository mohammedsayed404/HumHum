using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;
using Shared.ViewModels;

namespace Presentation.Controllers;


[Authorize(Roles = Roles.RestaurantManager)]
public class ProductCategoryController : Controller
{

    private readonly IServiceManager _serviceManager;
    private readonly string cartId;
    public ProductCategoryController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
        cartId = _serviceManager.UserServices.Id!;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var categories = await _serviceManager.ProductService.GetAllCategoriesAsync();

        return View(categories);
    }

    [AllowAnonymous]
    public async Task<IActionResult> ShowProductsByCategory(int categoryId)
    {

        try
        {
            var products = await _serviceManager.ProductService
                                                .GetAllProductsAsync(new ProductParameterRequest(null, categoryId));


            var customerCart =
                await _serviceManager.CartService.GetCustomerCartAsync(cartId);

            var items = customerCart.Items;


            var productsWithQuantity = new ProductToRestaurantWithQuantityViewModel()
            {
                Products = products.ToList(),
                RestaurantName = products[0].Restaurant,
                Quantity = Enumerable.Repeat(0, products.Count).ToList()
            };

            if (items.Count != 0)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    productsWithQuantity.Quantity[i] =
                        items.FirstOrDefault(item => item.Id == products[i].Id)?.Quantity ?? 0;
                }
            }



            return View(productsWithQuantity);

        }
        catch
        {
            return View();
        }





    }



    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id, string viewName = nameof(Details))
    {
        if (!id.HasValue) return BadRequest();

        var productCategory = await _serviceManager.ProductCategoryService.GetProductCategoryByIdAsync(id.Value);

        if (productCategory is null) return NotFound();

        return View(viewName, productCategory);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ProductCategoryToCreationViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var created = await _serviceManager.ProductCategoryService.CreateProductCategoryAsync(model);

        if (created > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "cant' add product category pls try again later");
            return View(model);
        }

    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id,
       [FromServices] IUnitOfWork _unitOfWork, [FromServices] IMapper _mapper)
    {
        if (!id.HasValue) return BadRequest();

        var productCategory = await _unitOfWork.GetRepository<ProductCategory, int>().GetByIdAsync(id.Value);

        if (productCategory is null) return NotFound();


        var mappedProductCategory = _mapper.Map<ProductCategoryToUpdateViewModel>(productCategory);


        return View(mappedProductCategory);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, ProductCategoryToUpdateViewModel model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid) return View(model);

        var updated = await _serviceManager.ProductCategoryService.UpdateProductCategoryAsync(model);

        if (updated > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "can't Update product Category pls try again later");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
        => await Details(id, nameof(Delete));


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted = await _serviceManager.ProductCategoryService.DeleteProductCategoryAsync(id);

        if (deleted > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "can't delete product category pls try again later");

            return await Details(id, nameof(Delete));
        }
    }

}
