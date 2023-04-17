using Desapegando.Application.Models;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        var condominoList = await _condominoRepository.Read();
        var condomino = condominoList.Where(x => x.Email == condominoLoginViewModel.Email).FirstOrDefault();

        if (condomino != null)
        {
            if (condomino.Ativo)
            {
                var result = await _signInManager.PasswordSignInAsync(condominoLoginViewModel.Email, condominoLoginViewModel.Senha, false, false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário não ativado pelo síndico.");

                return View(condominoLoginViewModel);
            }
        }
        else
        {
            // Adicionar erro de problema
            return View();
        }

    }
}