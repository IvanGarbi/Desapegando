using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Desapegando.Application.Controllers;

public class LoginController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ICondominoRepository _condominoRepository;

    public LoginController(SignInManager<IdentityUser> signInManager, ICondominoRepository condominoRepository)
    {
        _signInManager = signInManager;
        _condominoRepository = condominoRepository;
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

        var condomino = await _condominoRepository.ReadWithExpression(x => x.Email == condominoLoginViewModel.Email);

        if (condomino != null)
        {
            if (condomino.Ativo)
            {
                var result = await _signInManager.PasswordSignInAsync(condominoLoginViewModel.Email, condominoLoginViewModel.Senha, false, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
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