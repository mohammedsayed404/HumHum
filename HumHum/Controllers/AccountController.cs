using Domain.Contracts;
using Domain.Entities.Identity;
using HumHum.SMTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.ViewModels;
using System.Security.Claims;

namespace HumHum.Controllers;
public class AccountController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    //Register , login , Forget Pass , 

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public AccountController(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             SignInManager<ApplicationUser> signInManager,
                             IEmailSender emailSender)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    // GET: /Account/Register
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
            //var customerRole = await _roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer);
                // Add custom claims
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, model.Email));

                await _signInManager.SignInAsync(user, isPersistent: true);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        return View(model);
    }

    // GET: /Account/Login
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", "Invalid login attempt.");

            }

        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return RedirectToAction("ForgotPasswordConfirmation");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action(
            "ResetPassword", "Account",
            new { token, email = model.Email },
            protocol: HttpContext.Request.Scheme);

        await _emailSender.SendEmailAsync(
            model.Email,
            "Reset Your Password",
            $"Click here to reset your password: <a href='{callbackUrl}'>link</a>");

        return RedirectToAction("ForgotPasswordConfirmation");
    }


    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (token == null || email == null)
            return BadRequest("A token and email must be supplied for password reset.");

        return View(new ResetPasswordViewModel { Token = token, Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return RedirectToAction("ResetPasswordConfirmation");

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
            return RedirectToAction("ResetPasswordConfirmation");

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
        returnUrl ??= Url.Content("~/");

        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
            return RedirectToAction(nameof(Login));
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction(nameof(Login));

        // Try to sign in the user with this external login provider
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            // If user doesn't have an account, prompt to create one
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
        }
    }

    [HttpGet]
    public IActionResult ExternalLoginConfirmation(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    public IActionResult AccessDenied() => View();

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }

}


