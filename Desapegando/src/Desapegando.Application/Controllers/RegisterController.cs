using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Models;
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
                              INotificador notificador) : base(httpClient, settings, notificador)
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
        ModelState.ClearValidationState("ImageFileName");
        ModelState.MarkFieldValid("ImageFileName");

        if (!ModelState.IsValid)
        {
            return View(condominoRegisterViewModel);
        }


        if (condominoRegisterViewModel.ImageUpload == null)
        {
            ModelState.AddModelError(string.Empty, "É obrigatório inserir uma imagem de perfil.");
            return View(condominoRegisterViewModel);
        }

        var imgPrefixo = Guid.NewGuid() + "_";
        if (!await UploadArquivo(condominoRegisterViewModel.ImageUpload, imgPrefixo))
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a imagem de perfil.");
            return View(condominoRegisterViewModel);
        }

        condominoRegisterViewModel.ImageFileName = imgPrefixo + condominoRegisterViewModel.ImageUpload.FileName;

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


            bool result = await DeletarArquivo(condominoRegisterViewModel.ImageFileName);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a imagem de perfil.");
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

    #region MétodosPrivados
    private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagens", imgPrefixo + arquivo.FileName);

        //verificar se o arquivo já existe no diretório
        if (System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
            return false;
        }


        // gravando em "disco"
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }

    private async Task<bool> DeletarArquivo(string imagem)
    {
        if (imagem.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imagem);

        //verificar se o arquivo já existe no diretório
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            return true;
        }

        return false;
    }
    #endregion

}