using Desapegando.Application.Models;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class LoginController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CondominoLoginViewModel condominoLoginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(condominoLoginViewModel);
        }

        var result = await _signInManager.PasswordSignInAsync(condominoLoginViewModel.Email, condominoLoginViewModel.Senha, false, false);

        return RedirectToAction("Index", "Home");
    }
}