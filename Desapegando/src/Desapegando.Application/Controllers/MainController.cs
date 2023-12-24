using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Desapegando.Application.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using NuGet.Configuration;
using Microsoft.Extensions.Options;
using Desapegando.Application.Extensions;

namespace Desapegando.Application.Controllers;

[Authorize]
public abstract class MainController : Controller
{
    //protected readonly INotificador _notificador;

    //public MainController(INotificador notificador)
    //{
    //    _notificador = notificador;
    //}

    protected bool VerifyResponseErros(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            // adicionar erros quando necessário
            case 400:
                return false;
            case 500:
                return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
    }

    protected async Task Login(UserResponseAuth response)
    {
        var token = GetToken(response.Data.AccessToken);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", response.Data.AccessToken));
        claims.AddRange(token.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private static JwtSecurityToken GetToken(string jwtToken)
    {
        return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
    }
}