using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;
using Shared.ViewModels;

namespace Presentation.Controllers;

public class ProductController : Controller
{
    private readonly IServiceManager _serviceManager;

    private readonly string cartId;

    public ProductController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
        cartId = _serviceManager.UserServices.Id!;
    }

    public async Task<IActionResult> Index(ProductParameterRequest request)
    //public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 4)
    {
        //if (User.IsInRole(Roles.RestaurantManager))
        //{
        //    return RedirectToAction(nameof())
        //}
        var products = await _serviceManager.ProductService.GetAllProductsAsync(request);
        var customerCart = await _serviceManager.CartService.GetCustomerCartAsync(cartId);

        var items = customerCart.Items;
        var productsWithQuantity = new ProductToRestaurantWithQuantityViewModel();
        var viewModel = new ProductToRestaurantWithQuantityViewModel();

        //requst.search != null
        if (request.Search != null)
        {
            if (products?.Any() == true)
            {
                productsWithQuantity.Products = products.ToList();
                productsWithQuantity.RestaurantName = products[0].Restaurant;
                productsWithQuantity.Quantity = Enumerable.Repeat(0, products.Count).ToList();

                if (items.Count != 0)
                {
                    for (int i = 0; i < products.Count; i++)
                    {
                        productsWithQuantity.Quantity[i] =
                            items.FirstOrDefault(item => item.Id == products[i].Id)?.Quantity ?? 0;
                    }
                }
            }
        }
        //else + pagedPage
        else
        {
            var pagedProducts = products
            .Skip((request.pageNumber - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();
            viewModel.Products = pagedProducts;
            viewModel.RestaurantName = pagedProducts[0].Restaurant;
            viewModel.Quantity = Enumerable.Repeat(0, pagedProducts.Count).ToList();
            viewModel.TotalPages = (int)Math.Ceiling(products.Count / (double)request.pageSize);
            viewModel.CurrentPage = request.pageNumber;
            if (items.Count != 0)
            {
                for (int i = 0; i < pagedProducts.Count; i++)
                {
                    viewModel.Quantity[i] =
                        items.FirstOrDefault(item => item.Id == pagedProducts[i].Id)?.Quantity ?? 0;
                }
            }
        }

        if (request.Search == null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_ProductCardsPartial", viewModel);
        }
        else if (request.Search != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("ProductsSearchPartialView", productsWithQuantity);
        }

        return View(viewModel);
        //return View(viewModel);
    }
    public async Task<IActionResult> ShowAll(ProductParameterRequest request)
    {
        var products = await _serviceManager.ProductService.GetAllProductsAsync(request);

        return View(products);
    }
    public async Task<IActionResult> Details(int? id, string viewName = nameof(Details))
    {
        if (!id.HasValue) return BadRequest();

        var product = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);

        if (product is null) return NotFound();

        return View(viewName, product);
    }


    [HttpGet]
    //[Authorize(Roles = Roles.Administrator)]
    public IActionResult Create(int restaurantId)
    {
        ViewBag.restaurantId = restaurantId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductToCreationViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var created = await _serviceManager.ProductService.CreateProductAsync(model);

        if (created > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "cant' add product pls try again later");
            return View(model);
        }

    }


    [HttpGet]
    public async Task<IActionResult> Edit(int? id,
        [FromServices] IUnitOfWork _unitOfWork, [FromServices] IMapper _mapper)
    {
        if (!id.HasValue) return BadRequest();

        var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id.Value);

        if (product is null) return NotFound();


        var mappedProduct = _mapper.Map<ProductToUpdateViewModel>(product);


        return View(mappedProduct);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, ProductToUpdateViewModel model)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid) return View(model);

        var updated = await _serviceManager.ProductService.UpdateProductAsync(model);

        if (updated > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "can't Update product pls try again later");
            return View(model);
        }
    }




    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
        => await Details(id, nameof(Delete));



    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted = await _serviceManager.ProductService.DeleteProductAsync(id);

        if (deleted > 0)
            return RedirectToAction(nameof(Index));
        else
        {
            ModelState.AddModelError(string.Empty, "can't delete product pls try again later");

            return await Details(id, nameof(Delete));
        }
    }
}
