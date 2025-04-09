using HumHum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.Diagnostics;

namespace HumHum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IServiceManager ServiceManager { get; }

        public HomeController(ILogger<HomeController> logger,
            IServiceManager serviceManager


            )
        {
            _logger = logger;
            ServiceManager = serviceManager;
        }

        public IActionResult Index()
        {

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
