using AutoMapper;
using Desapegando.Application.ViewModels;
using Desapegando.Application.Services;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class AdministradorController : MainController
{
    private readonly ICondominoRepository _condominoRepository;
    private readonly ICondominoService _condominoService;
    private readonly IMapper _mapper;

    private readonly IEmailSender _emailSender;
    public AdministradorController(ICondominoRepository condominoRepository, IMapper mapper, ICondominoService condominoService, IEmailSender emailSender)
    {
        _condominoRepository = condominoRepository;
        _mapper = mapper;
        _condominoService = condominoService;
        _emailSender = emailSender;
    }

    public async Task<IActionResult> NovosCondominos()
    {
        var condominosInativos = await _condominoRepository.ReadWithExpressionList(x => x.Ativo == false);

        //var condominosInativos = condominos.Where(x => x.Ativo == false);

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

        try
        {
            await _emailSender.SendEmailAsync(condomino.Email, "Condômino aprovado", "O seu cadastro em Desapegando já foi aprovado! Já é possível realizar o login e desfrutar da plataforma!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        return RedirectToAction("NovosCondominos", "Administrador");
    }

}