using Domain.Entities.Identity;
using HumHum.Mock;
using HumHum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Diagnostics;

namespace HumHum.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceManager _serviceManager;
        private readonly MockCurrentUser _mockCurrentUser;
        private readonly UserManager<ApplicationUser> _userManager;



        public HomeController(ILogger<HomeController> logger,
            IServiceManager serviceManager,
            MockCurrentUser mockCurrentUser,
            UserManager<ApplicationUser> userManager

            )
        {
            _logger = logger;
            _serviceManager = serviceManager;
            _mockCurrentUser = mockCurrentUser;
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
            #endregion

            var topProducts = await _serviceManager.ProductService
                .GetTopRatingProductsAsync(8);

            return View(topProducts);

        }

        public IActionResult Privacy()
        {


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
