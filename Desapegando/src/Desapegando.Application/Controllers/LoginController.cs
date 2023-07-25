using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

[AllowAnonymous]
public class LoginController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ICondominoRepository _condominoRepository;

    public LoginController(SignInManager<IdentityUser> signInManager, 
                           ICondominoRepository condominoRepository, 
                           INotificador notificador) : base(notificador)
    {
        _signInManager = signInManager;
        _condominoRepository = condominoRepository;
    }

    [Route("")]
    [Route("Login")]
    public async Task<IActionResult> Index(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Index(CondominoLoginViewModel condominoLoginViewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            return View(condominoLoginViewModel);
        }

        var condomino = await _condominoRepository.ReadWithExpression(x => x.Email == condominoLoginViewModel.Email);

        if (condomino != null)
        {
            if (condomino.Ativo)
            {
                var result = await _signInManager.PasswordSignInAsync(condominoLoginViewModel.Email, condominoLoginViewModel.Senha, false, true);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

                    return LocalRedirect(returnUrl);
                    //return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ViewData.ModelState.AddModelError(string.Empty, "Usuário temporariamente bloqueado por tentativas inválidas.");
                    return View(condominoLoginViewModel);
                }

                ViewData.ModelState.AddModelError(string.Empty, "Usuário ou Senha incorretos.");
                return View(condominoLoginViewModel);
            }
            else
            {
                // Adicionando na ViewData para ser mostrado no View Component.
                ViewData.ModelState.AddModelError(string.Empty, "Usuário não ativado pelo síndico.");

                return View(condominoLoginViewModel);
            }
        }
        else
        {
            // Adicionar erro de problema
            ViewData.ModelState.AddModelError(string.Empty, "Usuário ou Senha incorretos.");
            return View();
        }

    }

    public async Task<IActionResult> SignOut()
    {
        var result = _signInManager.SignOutAsync();

        if (result.IsCompletedSuccessfully)
        {
            return RedirectToAction("Index", "Login");
        }
        else
        {
            // Verificar tratamento de erros...
            return RedirectToAction("Index", "Home");
        }

    }
}