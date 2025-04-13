using Domain.Entities.Identity;
using HumHum.Mock;
using HumHum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Diagnostics;

namespace HumHum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MockCurrentUser _mockCurrentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public IServiceManager ServiceManager { get; }

        public HomeController(ILogger<HomeController> logger,
            IServiceManager serviceManager,
            MockCurrentUser mockCurrentUser,
            UserManager<ApplicationUser> userManager

            )
        {
            _logger = logger;
            ServiceManager = serviceManager;
            _mockCurrentUser = mockCurrentUser;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {


            //var seka = await _userManager.FindByIdAsync(_mockCurrentUser?.Id);

            //Console.WriteLine(seka);


            return View();
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
