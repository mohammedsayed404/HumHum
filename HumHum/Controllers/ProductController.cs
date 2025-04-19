using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;
using Shared.ViewModels;

namespace HumHum.Controllers;

public class ProductController : Controller
{
    private readonly IServiceManager _serviceManager;

    private readonly string cartId;

    public ProductController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
        cartId = _serviceManager.UserServices.Id!;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 4)
    {
        var products = await _serviceManager.ProductService.GetAllProductsAsync();
        var customerCart = await _serviceManager.CartService.GetCustomerCartAsync(cartId);
        var items = customerCart.Items;

        var pagedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var quantity = Enumerable.Repeat(0, pagedProducts.Count).ToList();

        if (items.Count != 0)
        {
            for (int i = 0; i < pagedProducts.Count; i++)
            {
                quantity[i] = items.FirstOrDefault(item => item.Id == pagedProducts[i].Id)?.Quantity ?? 0;
            }
        }

        var viewModel = new ProductToRestaurantWithQuantityViewModel
        {
            Products = pagedProducts,
            RestaurantName = products[0].Name,
            Quantity = quantity,
            TotalPages = (int)Math.Ceiling(products.Count / (double)pageSize),
            CurrentPage = pageNumber
        };

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView("_ProductCardsPartial", viewModel);
        }

        return View(viewModel);
    }


    public async Task<IActionResult> Details(int? id, string viewName = nameof(Details))
    {
        if (!id.HasValue) return BadRequest();

        var product = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);

        if (product is null) return NotFound();

        return View(viewName, product);

    }


    [HttpGet]
    //[Authorize(Roles =Roles.Administrator)]
    public IActionResult Create() => View();

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
