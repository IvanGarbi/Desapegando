using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

[AllowAnonymous]
public class RegisterController : MainController
{
    private readonly HttpClient _httpClient;

    //private readonly ICondominoService _condominoService;
    //private readonly UserManager<IdentityUser> _userManager;
    //private readonly IMapper _mapper;

    public RegisterController(//UserManager<IdentityUser> userManager,
                              HttpClient httpClient,
                              IOptions<AppSettings> settings,
                              //ICondominoService condominoService, 
                              //IMapper mapper, 
                              INotificador notificador) : base(notificador)
    {
        //_userManager = userManager;
        //_condominoService = condominoService;
        //_mapper = mapper;

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



public class UserResponseAuth
{
    public bool Success { get; set; }
    public DataAuth Data { get; set; }
}

public class DataAuth
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken UserToken { get; set; }
    public ResponseResult ResponseResult { get; set; }
}

public class UserToken
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserClaim> Claims { get; set; }
}

public class UserClaim
{
    public string Value { get; set; }
    public string Type { get; set; }
}

public class ResponseResult
{
    public ResponseResult()
    {
        Errors = new ResponseErrorMessages();
    }

    //public string Title { get; set; }
    //public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; }
}

public class ResponseErrorMessages
{
    public ResponseErrorMessages()
    {
        Messages = new List<string>();
    }

    public List<string> Messages { get; set; }
}