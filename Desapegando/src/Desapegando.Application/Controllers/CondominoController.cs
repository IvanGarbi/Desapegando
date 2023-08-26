using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

public class CondominoController : MainController
{
    private readonly HttpClient _httpClient;

    private readonly ICondominoRepository _condominoRepository;
    private readonly ICondominoService _condominoService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public CondominoController(HttpClient httpClient,
                               IOptions<AppSettings> settings,
                               ICondominoRepository condominoRepository, 
                               IMapper mapper, 
                               UserManager<IdentityUser> userManager, 
                               ICondominoService condominoService,
                               INotificador notificador) : base(notificador)
    {
        _condominoRepository = condominoRepository;
        _mapper = mapper;
        _userManager = userManager;
        _condominoService = condominoService;

        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
    }

    //public async Task<IActionResult> Index()
    //{
    //    var teste = await _condominoRepository.ReadById(Guid.Parse(_userManager.GetUserId(User)));

    //    return View(_mapper.Map<CondominoViewModel>(await _condominoRepository.ReadById(Guid.Parse(_userManager.GetUserId(User)))));
    //}

    public async Task<IActionResult> Index()
    {
        var condomino = await _condominoRepository.ReadById(Guid.Parse(User.FindFirst("sub")?.Value));


        var response = await _httpClient.GetAsync("Condomino/Condomino/" + condomino.Id);

        GetCondominoResponseId employeesResponse;

        employeesResponse = await DeserializeObjectResponse<GetCondominoResponseId>(response);

        return View(employeesResponse.Data);
    }

    //[HttpPost]
    //public async Task<IActionResult> Index(CondominoViewModel condominoViewModel)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(condominoViewModel);
    //    }

    //    var condomino = _mapper.Map<Condomino>(condominoViewModel);

    //    var validator = new CondominoValidation();
    //    var resultValidation = validator.Validate(condomino);

    //    if (!resultValidation.IsValid)
    //    {
    //        foreach (var error in resultValidation.Errors)
    //        {
    //            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
    //        }
    //    }

    //    await _condominoService.Update(condomino);

    //    return View(condominoViewModel);
    //}

    [HttpPost]
    public async Task<IActionResult> Index(CondominoViewModel condominoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(condominoViewModel);
        }

        var condominoContent = new StringContent(
            JsonSerializer.Serialize(condominoViewModel),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PutAsync("Condomino/Condomino/" + condominoViewModel.Id, condominoContent);

        CondominoResponse condominoResponse;

        if (!VerifyResponseErros(response))
        {
            condominoResponse = new CondominoResponse
            {
                Success = false,
                Data = new DataCondomino
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in condominoResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(condominoViewModel);
        }

        condominoResponse = await DeserializeObjectResponse<CondominoResponse>(response);

        return View(condominoViewModel);

        //TempData["Success"] = "Funcionário salvo!";

        //return RedirectToAction("Index", "Employees");
    }
}