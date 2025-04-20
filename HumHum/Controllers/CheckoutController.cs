using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.OrderModule;
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
            var mappedUserAddress = new OrderAddressToReturnDto(userAddress.FirstName, userAddress.LastName, userAddress.Street, userAddress.City, userAddress.Country);
            var orderModel = new OrderToCreationViewModel(userId, 0, mappedUserAddress); 
            return View(orderModel);
        }



        #region Old
        //[HttpPost]
        //public async Task<IActionResult> SetDefaultAddress(AddressToUpdateViewModel model)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userId))
        //        return RedirectToAction("Login", "Account");


        //    await _serviceManager.UserServices.UpdateUserAddressAsync(model);

        //    return RedirectToAction("Index");
        //}

        ////Step02 DeliveryMethod
        //[HttpPost]
        //public async Task<IActionResult> Next(OrderToCreationViewModel orderToCreateModel)
        //{
        //    Console.WriteLine(orderToCreateModel);
        //    var deliveryMethods = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();

        //    var model = new DeliveryMethodViewModel
        //    {
        //        DeliveryMethods = deliveryMethods.ToList()
        //    };

        //    return View("DeliveryMethod", model);
        //} 
        #endregion

        //KafagaTest // Payment //Youssef Will Comelete from here
        //public async Task<IActionResult> CreateOrUpdatePayment(OrderToCreationViewModel orderToCreate)
        //{
        //    Console.WriteLine(orderToCreate);
        //    //var deliveryMethods = await _serviceManager.OrderService.GetAllDeliveryMethodsAsync();

        //    //var model = new DeliveryMethodViewModel
        //    //{
        //    //    DeliveryMethods = deliveryMethods.ToList()
        //    //};

        //    return View("PaymentSummery");
        //}

        #region Old

        ////step03
        //[HttpPost]
        //public IActionResult ConfirmDeliveryMethod(int SelectedDeliveryMethodId)
        //{
        //    TempData["SelectedDeliveryMethodId"] = SelectedDeliveryMethodId;

        //    return RedirectToAction("PaymentSummary");
        //} 
        #endregion

    }
}
