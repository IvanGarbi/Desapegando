using AutoMapper;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class CondominoController : MainController
{
    private readonly ICondominoRepository _condominoRepository;
    private readonly ICondominoService _condominoService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public CondominoController(ICondominoRepository condominoRepository, 
                               IMapper mapper, 
                               UserManager<IdentityUser> userManager, 
                               ICondominoService condominoService,
                               INotificador notificador) : base(notificador)
    {
        _condominoRepository = condominoRepository;
        _mapper = mapper;
        _userManager = userManager;
        _condominoService = condominoService;
    }

    public async Task<IActionResult> Index()
    {
        var teste = await _condominoRepository.ReadById(Guid.Parse(_userManager.GetUserId(User)));

        return View(_mapper.Map<CondominoViewModel>(await _condominoRepository.ReadById(Guid.Parse(_userManager.GetUserId(User)))));
    }

    [HttpPost]
    public async Task<IActionResult> Index(CondominoViewModel condominoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(condominoViewModel);
        }

        var condomino = _mapper.Map<Condomino>(condominoViewModel);

        var validator = new CondominoValidation();
        var resultValidation = validator.Validate(condomino);

        if (!resultValidation.IsValid)
        {
            foreach (var error in resultValidation.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        await _condominoService.Update(condomino);

        return View(condominoViewModel);
    }
}