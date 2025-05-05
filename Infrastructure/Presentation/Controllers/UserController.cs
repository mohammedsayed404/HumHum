using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.ViewModels;

namespace Presentation.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IServiceManager _serviceManager;
    private readonly IMapper _mapper;

    public UserController(IServiceManager serviceManager, IMapper mapper)
    {
        _serviceManager = serviceManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> MyAccount()
    {


        var user = await _serviceManager.UserServices.GetCurrentUserWithAddressAsync();


        return View(_mapper.Map<ApplicationUserToUpdateViewModule>(user));

    }



    [HttpPost]
    public async Task<IActionResult> UpdateUserInfo(ApplicationUserToUpdateViewModule model)
    {

        var updated = await _serviceManager.UserServices.UpdateUserInformationAsync(model);


        if (updated)
            return RedirectToAction(nameof(MyAccount));

        return RedirectToAction(nameof(MyAccount));


    }



    [HttpGet]

    public async Task<IActionResult> GetUserOrdersAsync()
    {
        var userEmail = _serviceManager.UserServices.UserEmail;

        var orders = await _serviceManager.OrderService.GetOrdersForUserByEmailAsync(userEmail!);

        return View(orders);

    }


}
