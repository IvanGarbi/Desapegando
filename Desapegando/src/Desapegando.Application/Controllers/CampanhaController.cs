using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

public class CampanhaController : MainController
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public CampanhaController(HttpClient httpClient,
                              IOptions<AppSettings> settings,
                              IMapper mapper,
                              INotificador notificador) : base(notificador)
    {
        _mapper = mapper;
        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Criar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CampanhaViewModel campanhaViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(campanhaViewModel);
        }

        if (campanhaViewModel.ImagensUpload.Any() && campanhaViewModel.ImagensUpload.Count > 4)
        {
            ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
            return View(campanhaViewModel);
        }

        //

        var postCampanhaViewModel = _mapper.Map<PostCampanhaViewModel>(campanhaViewModel);

        postCampanhaViewModel.CondominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        postCampanhaViewModel.ImagensUploadNames = new List<string>();

        foreach (var imagem in campanhaViewModel.ImagensUpload)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(imagem, imgPrefixo))
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                return View(campanhaViewModel);
            }

            postCampanhaViewModel.ImagensUploadNames.Add(imgPrefixo + imagem.FileName);
        }

        //var produtoContentImagem = new MultipartFormDataContent();

        //foreach (var file in produtoViewModel.ImagensUpload)
        //{
        //    produtoContentImagem.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
        //}


        var campanhaContent = new StringContent(
                JsonSerializer.Serialize(postCampanhaViewModel),
                Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PostAsync("Campanha/Campanha/", campanhaContent);

        CampanhaResponse campanhaResponse;

        if (!VerifyResponseErros(response))
        {
            campanhaResponse = new CampanhaResponse
            {
                Success = false,
                Data = new DataCampanha
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in campanhaResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(campanhaViewModel);
        }

        campanhaResponse = await DeserializeObjectResponse<CampanhaResponse>(response);
        //

        //var produto = _mapper.Map<Produto>(produtoViewModel);

        //produto.CondominoId = Guid.Parse(_userManager.GetUserId(this.User));

        //await _produtoService.Create(produto);

        if (!_notificador.TemNotificacao())
        {
            //foreach (var imagem in produtoViewModel.ImagensUpload)
            //{
            //    var imgPrefixo = Guid.NewGuid() + "_";
            //    if (!await UploadArquivo(imagem, imgPrefixo))
            //    {
            //        await _produtoService.Delete(postProdutoViewModel.Id);

            //        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

            //        return View(produtoViewModel);
            //    }

            //    var produtoImagem = new ProdutoImagem();
            //    produtoImagem.FileName = imgPrefixo + imagem.FileName;
            //    produtoImagem.ProdutoId = postProdutoViewModel.Id;

            //    await _produtoImagemService.Create(produtoImagem);

            //}

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in _notificador.GetNotificacoes())
        {
            ModelState.AddModelError(error.Propriedade, error.Mensagem);
        }

        return View(campanhaViewModel);
    }

    public async Task<IActionResult> MinhasCampanhas()
    {
        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var response = await _httpClient.GetAsync("Campanha/Campanha/MinhasCampanhas/" + condominoId);

        GetMinhasCampanhasResponse campanhasResponse;

        campanhasResponse = await DeserializeObjectResponse<GetMinhasCampanhasResponse>(response);

        return View(campanhasResponse.Data);
    }

    public async Task<IActionResult> Editar(Guid id)
    {
        var response = await _httpClient.GetAsync("Campanha/Campanha/" + id);

        GetCampanhaResponseId campanhaResponse;

        //ProdutoResponse condominoResponse;

        //if (!VerifyResponseErros(response))
        //{
        //    condominoResponse = new ProdutoResponse
        //    {
        //        Success = false,
        //        Data = new DataProduto
        //        {
        //            ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
        //        }
        //    };

        //    foreach (var error in condominoResponse.Data.ResponseResult.Errors.Messages)
        //    {
        //        ModelState.AddModelError(string.Empty, error);
        //    }

        //    ViewBag.Error = "Ocorreu um erro ao salvar";

        //    return View();
        //}

        campanhaResponse = await DeserializeObjectResponse<GetCampanhaResponseId>(response);

        var campanha = _mapper.Map<Campanha>(campanhaResponse.Data);

        var updateCampanhaViewModel = _mapper.Map<UpdateCampanhaViewModel>(campanha);

        return View(updateCampanhaViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(UpdateCampanhaViewModel campanhaViewModel)
    {
        bool novasImagens = campanhaViewModel.ImagensUpload != null;

        if (!novasImagens)
        {
            ModelState.ClearValidationState("ImagensUpload");
            ModelState.MarkFieldValid("ImagensUpload");
        }

        if (!ModelState.IsValid)
        {
            return View(campanhaViewModel);
        }

        if (novasImagens && campanhaViewModel.ImagensUpload.Count > 4)
        {
            ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
            return View(campanhaViewModel);
        }


        var patchCampanhaViewModel = _mapper.Map<PatchCampanhaViewModel>(campanhaViewModel);

        patchCampanhaViewModel.CondominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        patchCampanhaViewModel.ImagensUploadNames = new List<string>();

        if (novasImagens)
        {
            foreach (var imagem in campanhaViewModel.ImagensUpload)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(imagem, imgPrefixo))
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                    return View(campanhaViewModel);
                }

                patchCampanhaViewModel.ImagensUploadNames.Add(imgPrefixo + imagem.FileName);
            }

            var responseCampanha = await _httpClient.GetAsync("Campanha/Campanha/" + campanhaViewModel.Id);

            GetCampanhaResponseId campanhaDb;

            campanhaDb = await DeserializeObjectResponse<GetCampanhaResponseId>(responseCampanha);


            if (!campanhaDb.Success)
            {
                return View(campanhaViewModel);
            }

            var listaProdutoImagensDb = campanhaDb.Data.CampanhaImagemViewModels;
            // Deletando imagens antigas
            foreach (var imagem in listaProdutoImagensDb)
            {
                bool result = await DeletarArquivo(imagem.FileName);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");
                }
            }
        }

        //
        var campanhaContent = new StringContent(
        JsonSerializer.Serialize(patchCampanhaViewModel),
        Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PatchAsync("Campanha/Campanha/", campanhaContent);

        CampanhaResponse campanhaResponse;

        if (!VerifyResponseErros(response))
        {
            campanhaResponse = new CampanhaResponse
            {
                Success = false,
                Data = new DataCampanha
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in campanhaResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(campanhaViewModel);
        }

        campanhaResponse = await DeserializeObjectResponse<CampanhaResponse>(response);
        //

        if (!_notificador.TemNotificacao())
        {
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in _notificador.GetNotificacoes())
        {
            ModelState.AddModelError(error.Propriedade, error.Mensagem);
        }

        return View(campanhaViewModel);
    }

    public async Task<IActionResult> Visualizar(Guid id)
    {
        var response = await _httpClient.GetAsync("Campanha/Campanha/" + id);

        GetCampanhaResponseId campanhaResponse;

        campanhaResponse = await DeserializeObjectResponse<GetCampanhaResponseId>(response);

        return View(campanhaResponse.Data);
    }

    public async Task<IActionResult> Deletar(Guid id)
    {
        var response = await _httpClient.DeleteAsync("Campanha/Campanha/" + id);

        CampanhaResponse campanhaResponse;

        if (!VerifyResponseErros(response))
        {
            campanhaResponse = new CampanhaResponse
            {
                Success = false,
                Data = new DataCampanha
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in campanhaResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View();
        }

        if (!_notificador.TemNotificacao())
        {
            return RedirectToAction("MinhasCampanhas");
        }

        return View();
    }

    public async Task<IActionResult> Campanhas()
    {
        var response = await _httpClient.GetAsync("Campanha/Campanha");

        GetAllCampanhaResponse campanhaResponse;

        campanhaResponse = await DeserializeObjectResponse<GetAllCampanhaResponse>(response);

        var campanhasDb = campanhaResponse.Data.Where(x => x.Ativo);

        ViewBag.Campanhas = Enumerable.Empty<GetCampanhaViewModel>();

        if (!campanhasDb.Any())
        {
            return View();
        }

        ViewBag.Campanhas = campanhasDb;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Campanhas(FiltrarCampanhaViewModel filtrarCampanhaViewModel)
    {
        if (filtrarCampanhaViewModel.Nome == null || string.IsNullOrEmpty(filtrarCampanhaViewModel.Nome))
        {
            return RedirectToAction("Campanhas");
        }

        var response = await _httpClient.GetAsync("Campanha/Campanha");

        GetAllCampanhaResponse campanhaResponse;

        campanhaResponse = await DeserializeObjectResponse<GetAllCampanhaResponse>(response);

        var campanhasDb = campanhaResponse.Data.Where(x => x.Nome.ToLower().Trim().Contains(filtrarCampanhaViewModel.Nome.ToLower().Trim()) && x.Ativo == true);

        ViewBag.Campanhas = Enumerable.Empty<GetCampanhaViewModel>();

        if (!campanhasDb.Any())
        {
            return View();
        }

        var campanhasViewModel = _mapper.Map<IEnumerable<GetCampanhaViewModel>>(campanhasDb);

        ViewBag.Campanhas = campanhasViewModel;

        return View();
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

    private static void MapearCampanha(Campanha campanha, UpdateCampanhaViewModel campanhaViewModel)
    {
        campanha.Nome = campanhaViewModel.Nome;
        campanha.Descricao = campanhaViewModel.Descricao;
        campanha.Ativo = campanhaViewModel.Ativo;
        campanha.NomeInstituicao = campanhaViewModel.NomeInstituicao;
        campanha.DataInicio = campanhaViewModel.DataInicio.Value;
        campanha.DataFinal = campanhaViewModel.DataFinal.Value;
        campanha.EmailResponsavel = campanhaViewModel.EmailResponsavel;
        campanha.LocalDeEncontro = campanhaViewModel.LocalDeEncontro;
        campanha.NomeResponsavel = campanhaViewModel.NomeResponsavel;
        campanha.TelefoneResponsavel = campanhaViewModel.TelefoneResponsavel;
    }
    #endregion
}

