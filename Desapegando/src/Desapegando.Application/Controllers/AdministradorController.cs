using AutoMapper;
using Desapegando.Application.Models;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class AdministradorController : MainController
{
    private readonly ICondominoRepository _condominoRepository;
    private readonly ICondominoService _condominoService;
    private readonly IMapper _mapper;
    public AdministradorController(ICondominoRepository condominoRepository, IMapper mapper, ICondominoService condominoService)
    {
        _condominoRepository = condominoRepository;
        _mapper = mapper;
        _condominoService = condominoService;
    }

    public async Task<IActionResult> NovosCondominos()
    {
        var condominos = await _condominoRepository.Read();

        var condominosInativos = condominos.Where(x => x.Ativo == false);

        return View(_mapper.Map<IEnumerable<CondominoInativoViewModel>>(condominosInativos));
    }

    //[HttpPost]
    public async Task<IActionResult> AtivarCondomino(Guid Id)
    {
        var condomino = await _condominoRepository.ReadById(Id);

        if (condomino == null)
        {
            return RedirectToAction("NovosCondominos", "Administrador");
        }

        condomino.Ativo = true;

        await _condominoService.Update(condomino);

        return RedirectToAction("NovosCondominos", "Administrador");
    }

}