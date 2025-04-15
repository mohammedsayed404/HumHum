using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using Service.Abstractions;
using Shared;

namespace HumHum.Controllers
{
    public class CartController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public CartController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> CartDetails(string id)
        {
            try
            {
                var cart = await _serviceManager.CartService.GetCustomerCartAsync(id);
                decimal totalPrice = 0;

                foreach(var item in cart.Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }

                ViewBag.TotalPrice = totalPrice;
                return View(cart);
            }
            catch
            {
               return RedirectToAction("Index", "Restaurant");
            }
        }
        public async Task<IActionResult> AddOne(int id)
        {
            const string cartId = "54150542-7e35-4fe3-9fdc-ee0a383d4f07";

            var customerCartDto = await _serviceManager.CartService.GetCustomerCartAsync(cartId);

            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);

            var oldCartItem = customerCartDto.Items.FirstOrDefault(item => item.Id == product.Id);

            if (oldCartItem is null)
            {
                throw new ProductNotFoundException(id);
            }

            var listOfNewItems = new List<CartItemDto>();

            var newQuantity = 0;

            decimal totalPrice = 0.00M;

            foreach (var item in customerCartDto.Items)
            {
                if (item.Id == product.Id)
                {
                    var cartItem = new CartItemDto(product.Id, product.Name, product.Image, product.Price,
                        oldCartItem.Quantity + 1 , product.Category, product.Restaurant);

                    newQuantity = cartItem.Quantity;

                    totalPrice += cartItem.Quantity * cartItem.Price;

                    listOfNewItems.Add(cartItem);
                }
                else
                {
                    totalPrice += item.Quantity * item.Price;

                    listOfNewItems.Add(item);
                }
            }

            //foreach (var item in listOfNewItems)
            //{
            //    totalPrice += item.Price * item.Quantity;
            //}

            customerCartDto = new CustomerCartDto(cartId, listOfNewItems);

            var res = _serviceManager.CartService.UpdateCustomerCartAsync(customerCartDto);

            return Json(new { success = true, message = "Product is increased by one", quantity = newQuantity, total = totalPrice });
        }

        public async Task<IActionResult> DecreaseOne(int id)
        {
            const string cartId = "54150542-7e35-4fe3-9fdc-ee0a383d4f07";

            var customerCartDto = await _serviceManager.CartService.GetCustomerCartAsync(cartId);

            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);

            var oldCartItem = customerCartDto.Items.FirstOrDefault(item => item.Id == product.Id);

            if (oldCartItem is null)
            {
                throw new ProductNotFoundException(id);
            }
            
            var listOfNewItems = new List<CartItemDto>();

            var newQuantity = 0;

            decimal totalPrice = 0;

            foreach (var item in customerCartDto.Items)
            {
                if (item.Id == product.Id)
                {
                    if (item.Quantity > 1)
                    {
                        var cartItem = new CartItemDto(product.Id, product.Name, product.Image, product.Price,
                            oldCartItem.Quantity - 1, product.Category, product.Restaurant);
                        
                        newQuantity = cartItem.Quantity;

                        totalPrice += cartItem.Quantity * cartItem.Price;

                        listOfNewItems.Add(cartItem);
                    }
                    else
                    {
                        newQuantity = 0;
                    }
                }
                else
                {
                    totalPrice += item.Quantity * item.Price;

                    listOfNewItems.Add(item);
                }
            }

            //foreach (var item in listOfNewItems)
            //{
            //    totalPrice += item.Price * item.Quantity;
            //}

            customerCartDto = new CustomerCartDto(cartId, listOfNewItems);

            var res = _serviceManager.CartService.UpdateCustomerCartAsync(customerCartDto);

            return Json(new { success = true, message = "Product is decreased by one" , quantity = newQuantity, total = totalPrice });
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            const string cartId = "54150542-7e35-4fe3-9fdc-ee0a383d4f07";

            var customerCartDto = await _serviceManager.CartService.GetCustomerCartAsync(cartId);

            var product = await _serviceManager.ProductService.GetProductByIdAsync(id);

            var oldCartItem = customerCartDto.Items.FirstOrDefault(item => item.Id == product.Id);

            if (oldCartItem is null)
            {
                throw new ProductNotFoundException(id);
            }

            var listOfNewItems = new List<CartItemDto>();

            decimal totalPrice = 0;

            foreach (var item in customerCartDto.Items)
            {
                if (item.Id != product.Id)
                {
                    totalPrice += item.Price * item.Quantity;

                    listOfNewItems.Add(item);
                }
            }

            //foreach (var item in listOfNewItems)
            //{
            //    totalPrice += item.Price * item.Quantity;
            //}

            customerCartDto = new CustomerCartDto(cartId, listOfNewItems);

            var res = _serviceManager.CartService.UpdateCustomerCartAsync(customerCartDto);

            return Json(new { success = true, message = "Product is deleted", isDeleted = 1, total = totalPrice});
        }

        public async Task<IActionResult> UpdateCart(int id)
        {
            const string cartId = "54150542-7e35-4fe3-9fdc-ee0a383d4f07";
            CustomerCartDto existingProductList;
            try
            {
                 existingProductList =
                    await _serviceManager.CartService.GetCustomerCartAsync(cartId);
            }
            catch
            {
                var cart = new CustomerCartDto(cartId, new List<CartItemDto>());
                
                existingProductList = await _serviceManager.CartService.UpdateCustomerCartAsync(cart);
            }

            var oldProducts = existingProductList.Items;

            var cartItemList = new List<CartItemDto>();

            cartItemList.AddRange(oldProducts);

            var Product = await _serviceManager.ProductService.GetProductByIdAsync(id);

            if (!cartItemList.Exists(p => p.Id == Product.Id))
            {
                var cartItem = new CartItemDto(Product.Id, Product.Name,
                    Product.Image, Product.Price, 1,
                    Product.Category, Product.Restaurant);

                cartItemList.Add(cartItem);

                //HttpContext.Session.SetInt32("CartCount", cartItemList.Count);
            }
            
            var customerCart = new CustomerCartDto(cartId, cartItemList);

            //var Dto = new CustomerCartDto("5", )
            ///Get the id from the button
            /// Query the database to get the product by id
            /// Convert the product to a productToReturnDto
            /// CartItemDto <<= productToReturnDto
            /// Create the CustomerCartDto("20" as the user id, add k)


            var res = _serviceManager.CartService.UpdateCustomerCartAsync(customerCart);

            return Json(new { success = true, message = "Product is added to cart" , cartCount = cartItemList.Count});
        }

        public async Task<IActionResult> GetCartCount()
        {
            const string cartId = "54150542-7e35-4fe3-9fdc-ee0a383d4f07";
            CustomerCartDto existingProductList;
            try
            {
                existingProductList =
                    await _serviceManager.CartService.GetCustomerCartAsync(cartId);
            }
            catch
            {
                var cart = new CustomerCartDto(cartId, new List<CartItemDto>());

                existingProductList = await _serviceManager.CartService.UpdateCustomerCartAsync(cart);
            }

            //var Dto = new CustomerCartDto("5", )
            ///Get the id from the button
            /// Query the database to get the product by id
            /// Convert the product to a productToReturnDto
            /// CartItemDto <<= productToReturnDto
            /// Create the CustomerCartDto("20" as the user id, add k)

            return Json(new { success = true, cartCount = existingProductList.Items.Count });
        }
    }
}
