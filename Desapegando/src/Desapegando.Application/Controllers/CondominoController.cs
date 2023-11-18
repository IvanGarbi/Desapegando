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
    SignInManager<IdentityUser> _signInManager;

    public CondominoController(HttpClient httpClient,
                               IOptions<AppSettings> settings,
                               SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager,
                               INotificador notificador) : base(httpClient, settings, notificador)
    {
        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        AdicionarJWTnoHeader();

        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var response = await _httpClient.GetAsync("Condomino/Condomino/" + condominoId);

        GetCondominoResponseId employeesResponse;

        employeesResponse = await DeserializeObjectResponse<GetCondominoResponseId>(response);

        return View(employeesResponse.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Index(CondominoViewModel condominoViewModel)
    {
        ModelState.ClearValidationState("ImageFileName");
        ModelState.MarkFieldValid("ImageFileName");
        ModelState.ClearValidationState("ImageUpload");
        ModelState.MarkFieldValid("ImageUpload");

        if (!ModelState.IsValid)
        {
            return View(condominoViewModel);
        }


        //

        if (condominoViewModel.ImageUpload != null)
        {
            condominoViewModel.NovaImagem = true;

            var imagemAtual = User.FindFirst("ProfilePicture").Value;
            bool result = await DeletarArquivo(imagemAtual);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a imagem de perfil.");
            }

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(condominoViewModel.ImageUpload, imgPrefixo))
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a imagem de perfil.");
                return View(condominoViewModel);
            }

            condominoViewModel.ImageFileName = imgPrefixo + condominoViewModel.ImageUpload.FileName;
        }
        else
        {
            condominoViewModel.ImageFileName = User.FindFirst("ProfilePicture").Value;
        }

        //

        AdicionarJWTnoHeader();

        var condominoContent = new StringContent(
            JsonSerializer.Serialize(condominoViewModel),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PatchAsync("Condomino/Condomino/" + condominoViewModel.Id, condominoContent);

        if (!condominoViewModel.NovaImagem)
        {
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
        }
        else
        {
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

            }

            userResponse = await DeserializeObjectResponse<UserResponseAuth>(response);


            // Fazer Login para atualizar o cookie
            await Login(userResponse);
        }

        return RedirectToAction("Index", "Home");

        //return View(condominoViewModel);

        //TempData["Success"] = "Funcionário salvo!";

        //return RedirectToAction("Index", "Employees");
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
