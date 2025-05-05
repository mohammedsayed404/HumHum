using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using Service.Abstractions;
using Shared.ViewModels;

namespace Presentation.Mock;


[Authorize]
public class MockUserController : Controller
{
    private readonly IServiceManager _serviceManager;
    private readonly IMapper _mapper;

    public MockUserController(IServiceManager serviceManager, IMapper mapper)
    {
        _serviceManager = serviceManager;
        _mapper = mapper;
    }

    ///Get Current User Data 
    ///Get User Orders 
    ///

    [HttpGet]
    public async Task<IActionResult> GetUserInfo()
    {
        var user = await _serviceManager.UserServices.GetCurrentUserAsync();

        return View(user);

    }


    [HttpGet]
    public async Task<IActionResult> UpdateUserAddress()
    {
        var address = await _serviceManager.UserServices.GetUserAddressAsync(_serviceManager?.UserServices?.Id ?? string.Empty);

        return View(_mapper.Map<AddressToUpdateViewModel>(address));

    }



    [HttpPost]
    public async Task<IActionResult> UpdateUserAddress(AddressToUpdateViewModel model)
    {
        var updated = await _serviceManager.UserServices.UpdateUserAddressAsync(model);


        if (updated > 0)
            return RedirectToAction(nameof(HomeController.Index), "Home");


        return Json(new { Error = "pls try again" });

    }






}
