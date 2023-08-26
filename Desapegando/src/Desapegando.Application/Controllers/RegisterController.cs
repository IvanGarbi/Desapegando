using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

[AllowAnonymous]
public class RegisterController : MainController
{
    private readonly HttpClient _httpClient;

    public RegisterController(HttpClient httpClient,
                              IOptions<AppSettings> settings,
                              INotificador notificador) : base(notificador)
    {
        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
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


        var registerContent = new StringContent(
                JsonSerializer.Serialize(condominoRegisterViewModel),
                Encoding.UTF8,
                "application/json");
        var response = await _httpClient.PostAsync("Auth/Auth/Register/", registerContent);

        UserResponseAuth userResponse;

        if (!VerifyResponseErros(response))
        {
            userResponse = new UserResponseAuth
            {
                Success = false,
                Data = new DataAuth
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };
            foreach (var error in userResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(condominoRegisterViewModel);

        }

        userResponse = await DeserializeObjectResponse<UserResponseAuth>(response);

        //// Fazer Login
        //await Login(userResponse);


        // Adicionando na TempData para ser mostrado no View Component.
        TempData["Sucesso"] = "Agora só falta o Síndico aprovar seu registro!";

        return RedirectToAction("Index", "Login");
    }

}