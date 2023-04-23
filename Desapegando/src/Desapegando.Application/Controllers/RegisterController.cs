using AutoMapper;
using Desapegando.Application.Models;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class RegisterController : MainController
{
    private readonly ICondominoService _condominoService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public RegisterController(UserManager<IdentityUser> userManager, ICondominoService condominoService, IMapper mapper)
    {
        _userManager = userManager;
        _condominoService = condominoService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CondominoRegisterViewModel condominoRegisterViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(condominoRegisterViewModel);
        }

        var user = new IdentityUser();
        user.Email = condominoRegisterViewModel.Email;
        user.PhoneNumber = condominoRegisterViewModel.Telefone;
        user.UserName = condominoRegisterViewModel.Email;
        user.EmailConfirmed = true;
        user.PhoneNumberConfirmed = true;

        var result = await _userManager.CreateAsync(user, condominoRegisterViewModel.Senha);

        if (result.Succeeded)
        {
            var identity = await _userManager.FindByEmailAsync(condominoRegisterViewModel.Email);
            var condomino = _mapper.Map<Condomino>(condominoRegisterViewModel);

            condomino.Id = Guid.Parse(identity.Id);

            condomino.Telefone = condomino.Telefone.Replace("-", "");
            condomino.Telefone = condomino.Telefone.Replace("(", "");
            condomino.Telefone = condomino.Telefone.Replace(")", "");
            condomino.Telefone = condomino.Telefone.Replace(" ", "");
            condomino.Cpf = condomino.Cpf.Replace(".", "");
            condomino.Cpf = condomino.Cpf.Replace("-", "");

            var validator = new CondominoValidation();
            var resultValidation = validator.Validate(condomino);

            if (resultValidation.IsValid)
            {
                // Adicionando na TempData para ser mostrado no View Component.
                TempData["Sucesso"] = "Agora só falta o Síndico aprovar seu registro!";

                await _condominoService.Create(condomino);
            }
            else
            {
                foreach (var error in resultValidation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                await _userManager.DeleteAsync(user);

                return View(condominoRegisterViewModel);
            }
        }
        else
        {
            // Tratativa de erro para e-mail duplicado, etc. Erros do Identity
            foreach (var identityError in result.Errors)
            {
                // Adicionando na ViewData para ser mostrado no View Component.
                ViewData.ModelState.AddModelError("", identityError.Description);
            }
            return View(condominoRegisterViewModel);
        }

        return RedirectToAction("Index", "Login");
    }

}