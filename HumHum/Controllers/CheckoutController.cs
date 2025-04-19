using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.ViewModels;
using System.Security.Claims;


namespace HumHum.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public CheckoutController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var email = _serviceManager.UserServices.UserEmail;

            var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            if (string.IsNullOrEmpty(email) || !emailValidator.IsValid(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var userAddress = await _serviceManager.UserServices.GetUserAddressAsync(userId);

            return View(userAddress);
        }



        [HttpPost]
        public async Task<IActionResult> SetDefaultAddress(AddressToUpdateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");


            await _serviceManager.UserServices.UpdateUserAddressAsync(model);

            return RedirectToAction("Index");
        }

        //Step02 DeliveryMethod
        [HttpPost]
        public async Task<IActionResult> Next()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();

            var model = new DeliveryMethodViewModel
            {
                DeliveryMethods = deliveryMethods.ToList()
            };

            return View("DeliveryMethod", model);
        }


        //step03
        [HttpPost]
        public IActionResult ConfirmDeliveryMethod(int SelectedDeliveryMethodId)
        {
            TempData["SelectedDeliveryMethodId"] = SelectedDeliveryMethodId;

            return RedirectToAction("PaymentSummary");
        }

    }
}
