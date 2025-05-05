using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Abstractions;
using Shared.ViewModels;
using System.Diagnostics;

namespace Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceManager _serviceManager;
        //private readonly MockCurrentUser _mockCurrentUser;
        private readonly UserManager<ApplicationUser> _userManager;



        public HomeController(ILogger<HomeController> logger,
            IServiceManager serviceManager
           /* MockCurrentUser mockCurrentUser*/,
            UserManager<ApplicationUser> userManager

            )
        {
            _logger = logger;
            _serviceManager = serviceManager;
            //_mockCurrentUser = mockCurrentUser;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {


            #region Kafaga Testing
            //var seka = await _userManager.FindByIdAsync(_mockCurrentUser?.Id);

            //Console.WriteLine(seka);

            //Console.WriteLine(User.Identity.Name);
            //Console.WriteLine($"ssssssssssssssssss{User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            //var user = await _userManager.GetUserAsync(User);
            //Console.WriteLine($"ssssssssssssssssss{user.UserName}");

            //_userManager.Users 

            //var userAddress = await _serviceManager.UserServices.GetUserAddressAsync
            //    (User.FindFirstValue(ClaimTypes.NameIdentifier)!);



            //var updated = await _serviceManager.UserServices.UpdateUserAddressAsync(
            //    new AddressToUpdateViewModel(1, "Mohamed", "Khafaga", "123 str city", "Alex", "USA", User.FindFirstValue(ClaimTypes.NameIdentifier)!)
            //    );

            //or using our service _serviceManager.UserServices.Id


            #endregion

            var topProducts = await _serviceManager.ProductService.GetTopRatingProductsAsync(8);
            var featuredRestaurants = await _serviceManager.ProductService.GetProductsWithFeaturedRestaurantsAsync(8);

            ////var restProdImg = topProducts.Where(p => p.Restaurant.Contains(featuredRestaurants[0].Name) .ToLower()).FirstOrDefault();
            //var restProdImg = topProducts.FirstOrDefault(p => p.Restaurant == "Hardee's");

            var viewModel = new TopProductAndFeaturedRestaurantsViewModel(topProducts, featuredRestaurants);

            return View(viewModel);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
