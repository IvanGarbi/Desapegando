using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Desapegando.Application.Extensions;
using System.Text.Json;
using System.Text;

namespace Desapegando.Application.Controllers;

[AllowAnonymous]
public class LoginController : MainController
{
    private readonly HttpClient _httpClient;

    public LoginController(HttpClient httpClient,
                           IOptions<AppSettings> settings,
                           INotificador notificador) : base(httpClient, settings, notificador)
    {
        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
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

        var loginContent = new StringContent(
            JsonSerializer.Serialize(condominoLoginViewModel),
            Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PostAsync("Auth/Auth/Login", loginContent);

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

            return View(condominoLoginViewModel);

        }

        userResponse = await DeserializeObjectResponse<UserResponseAuth>(response);

        // Fazer Login
        await Login(userResponse);

        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

        return LocalRedirect(returnUrl);
    }

    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Login");
    }
}