using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.OrderModule;

namespace HumHum.Mock
{
    public class MockPaymentController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public MockPaymentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> CreateOrUpdatePaymentIntent(OrderToCreationViewModel orderToCreate)
        {
            
            var UEmail = _serviceManager.UserServices.UserEmail;
            var res = await _serviceManager.OrderService.CreateOrderAsync(orderToCreate, UEmail!);
            Console.WriteLine(orderToCreate);
            return View();
        }
    }
}
