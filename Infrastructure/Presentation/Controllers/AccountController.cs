using Domain.Contracts;
using Domain.Entities.Identity;
using HumHum.SMTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.ViewModels;
using System.Security.Claims;

namespace Presentation.Controllers;
public class AccountController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    //Register , login , Forget Pass , 

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             SignInManager<ApplicationUser> signInManager,
                             IEmailSender emailSender,
                                     ILogger<AccountController> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
    }


    public IActionResult MyAccount() => View();


    // GET: /Account/Register
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if email is already taken
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View(model);
            }

            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var user = new ApplicationUser
            {
                UserName = $"{model.Address.FirstName}{model.Address.Id}",
                DisplayName = $"{model.Address.FirstName} {model.Address.LastName}",
                Email = model.Email,
                Address = model.Address
            };
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



    // External Login Actions
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
        try
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"External provider error: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogWarning("External login info not available");
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider
            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {LoginProvider} provider", info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }

            // Handle new user registration
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ??
                      info.Principal.FindFirstValue(ClaimTypes.GivenName);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Email claim not received from {LoginProvider}", info.LoginProvider);
                ModelState.AddModelError(string.Empty, "Email claim not received from external provider.");
                return RedirectToAction(nameof(Login));
            }

            // Check if user already exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // Add external login to existing account
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (addLoginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in addLoginResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(nameof(Login));
            }

            // Prepare view model for new user registration
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
            {
                Email = email,
                Name = name ?? email.Split('@')[0]
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in external login callback");
            ModelState.AddModelError(string.Empty, "An error occurred during external login");
            return RedirectToAction(nameof(Login));
        }
    }



    [HttpGet]
    public IActionResult ExternalLoginConfirmation(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(
        ExternalLoginConfirmationViewModel model,
        string returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("External login info not available during confirmation");
                ModelState.AddModelError(string.Empty, "Error loading external login information");
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View(model);
            }
            //var user = new ApplicationUser
            //{
            //    UserName = model.Email,
            //    Email = model.Email,
            //    DisplayName = model.Name,
            //    HasSeenTour = false,
            //    EmailConfirmed = true // Add this for external logins

            //};
            var user = new ApplicationUser
            {
                UserName = $"{model.Name.Replace(" ", "")}{model.Address.Id}",
                DisplayName = model.Name,
                Email = model.Email,
                Address = model.Address,
                EmailConfirmed = true,
                HasSeenTour = false
            };
            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                // Add role to user
                await _userManager.AddToRoleAsync(user, Roles.Customer);

                // Add external login
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (addLoginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created an account using {LoginProvider} provider", info.LoginProvider);
                    return RedirectToLocal(returnUrl);
                }

                foreach (var error in addLoginResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in external login confirmation");
            ModelState.AddModelError(string.Empty, "An error occurred while creating your account");
            return View(model);
        }
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
    // Add to AccountController
    public IActionResult Lockout() => View();


    // Action to mark user as completed the Tour guide

    [Authorize]
    public async Task<IActionResult> CompleteTour()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            user.HasSeenTour = true;
            await _userManager.UpdateAsync(user);
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }
}


