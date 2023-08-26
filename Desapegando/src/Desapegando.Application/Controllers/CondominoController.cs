using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

public class CondominoController : MainController
{
    private readonly HttpClient _httpClient;

    private readonly UserManager<IdentityUser> _userManager;

    public CondominoController(HttpClient httpClient,
                               IOptions<AppSettings> settings,
                               INotificador notificador) : base(notificador)
    {
        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var response = await _httpClient.GetAsync("Condomino/Condomino/" + condominoId);

        GetCondominoResponseId employeesResponse;

        employeesResponse = await DeserializeObjectResponse<GetCondominoResponseId>(response);

        return View(employeesResponse.Data);
    }

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