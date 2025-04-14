using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.ViewModels;

[Authorize(Roles = Roles.Administrator)]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    // GET: /Role/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Role/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var exists = await _roleManager.RoleExistsAsync(model.RoleName.Trim());
            if (!exists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName.Trim()));
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role created successfully!";
                    return RedirectToAction(nameof(Create));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError("", "Role already exists.");
            }
        }

        return View(model);
    }
}
