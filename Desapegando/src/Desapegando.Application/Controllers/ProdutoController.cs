﻿using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

public class ProdutoController : MainController
{
    private readonly HttpClient _httpClient;

    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoService _produtoService;
    private readonly IProdutoCurtidaService _produtoCurtidaService;
    private readonly IProdutoImagemService _produtoImagemService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public ProdutoController(HttpClient httpClient,
                             IOptions<AppSettings> settings, 
                             IProdutoRepository produtoRepository,
                             IProdutoService produtoService, 
                             IMapper mapper, 
                             UserManager<IdentityUser> userManager, 
                             IProdutoImagemService produtoImagemService,
                             IProdutoCurtidaService produtoCurtidaService,
                             INotificador notificador) : base(notificador)
    {
        _produtoRepository = produtoRepository;
        _produtoService = produtoService;
        _mapper = mapper;
        _userManager = userManager;
        _produtoImagemService = produtoImagemService;
        _produtoCurtidaService = produtoCurtidaService;

        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Criar()
    {
        ProdutoViewModel model = new ProdutoViewModel();
        return View(model);
    }

    //[HttpPost]
    //public async Task<IActionResult> Criar(ProdutoViewModel produtoViewModel)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(produtoViewModel);
    //    }

    //    if (produtoViewModel.ImagensUpload.Any() && produtoViewModel.ImagensUpload.Count > 4)
    //    {
    //        ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
    //        return View(produtoViewModel);
    //    }

    //    var produto = _mapper.Map<Produto>(produtoViewModel);

    //    produto.CondominoId = Guid.Parse(_userManager.GetUserId(this.User));

    //    await _produtoService.Create(produto);

    //    if (!_notificador.TemNotificacao())
    //    {
    //        foreach (var imagem in produtoViewModel.ImagensUpload)
    //        {
    //            var imgPrefixo = Guid.NewGuid() + "_";
    //            if (!await UploadArquivo(imagem, imgPrefixo))
    //            {
    //                await _produtoService.Delete(produto.Id);

    //                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

    //                return View(produtoViewModel);
    //            }

    //            var produtoImagem = new ProdutoImagem();
    //            produtoImagem.FileName = imgPrefixo + imagem.FileName;
    //            produtoImagem.ProdutoId = produto.Id;

    //            await _produtoImagemService.Create(produtoImagem);

    //        }

    //        return RedirectToAction("Index", "Home");
    //    }

    //    foreach (var error in _notificador.GetNotificacoes())
    //    {
    //        ModelState.AddModelError(error.Propriedade, error.Mensagem);
    //    }

    //    return View(produtoViewModel);
    //}

    [HttpPost]
    public async Task<IActionResult> Criar(ProdutoViewModel produtoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(produtoViewModel);
        }

        if (produtoViewModel.ImagensUpload.Any() && produtoViewModel.ImagensUpload.Count > 4)
        {
            ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
            return View(produtoViewModel);
        }

        //

        var postProdutoViewModel = _mapper.Map<PostProdutoViewModel>(produtoViewModel);

        postProdutoViewModel.CondominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        postProdutoViewModel.ImagensUploadNames = new List<string>();

        foreach (var imagem in produtoViewModel.ImagensUpload)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(imagem, imgPrefixo))
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                return View(produtoViewModel);
            }
            
            postProdutoViewModel.ImagensUploadNames.Add(imgPrefixo + imagem.FileName);
        }

        //var produtoContentImagem = new MultipartFormDataContent();

        //foreach (var file in produtoViewModel.ImagensUpload)
        //{
        //    produtoContentImagem.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
        //}


        var produtoContent = new StringContent(
                JsonSerializer.Serialize(postProdutoViewModel),
                Encoding.UTF8,
                "application/json");

        var response = await _httpClient.PostAsync("Produto/Produto/", produtoContent);

        ProdutoResponse produtoResponse;

        if (!VerifyResponseErros(response))
        {
            produtoResponse = new ProdutoResponse
            {
                Success = false,
                Data = new DataProduto
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in produtoResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(produtoViewModel);
        }

        produtoResponse = await DeserializeObjectResponse<ProdutoResponse>(response);
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

        return View(produtoViewModel);
    }

    public async Task<IActionResult> MeusProdutos()
    {
        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var response = await _httpClient.GetAsync("Produto/Produto/MeusProdutos/" + condominoId);

        GetMeusProdutoResponse produtosResponse;

        produtosResponse = await DeserializeObjectResponse<GetMeusProdutoResponse>(response);

        return View(produtosResponse.Data);

    }

    //public async Task<IActionResult> Editar(Guid id)
    //{
    //    var produtoDb = await _produtoRepository.ReadById(id);

    //    if (produtoDb == null)
    //    {
    //        return View();
    //    }

    //    var produtoViewModel = _mapper.Map<UpdateProdutoViewModel>(produtoDb);

    //    return View(produtoViewModel);
    //}

    public async Task<IActionResult> Editar(Guid id)
    {
        var response = await _httpClient.GetAsync("Produto/Produto/" + id);

        GetProdutoResponseId produtoResponse;

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

        produtoResponse = await DeserializeObjectResponse<GetProdutoResponseId>(response);

        var produto = _mapper.Map<Produto>(produtoResponse.Data);

        var updateProdutoViewModel = _mapper.Map<UpdateProdutoViewModel>(produto);

        return View(updateProdutoViewModel);
    }

    //[HttpPost]
    //public async Task<IActionResult> Editar(UpdateProdutoViewModel produtoViewModel)
    //{
    //    var novasImagens = produtoViewModel.ImagensUpload != null;

    //    if (!novasImagens)
    //    {
    //        ModelState.ClearValidationState("ImagensUpload");
    //        ModelState.MarkFieldValid("ImagensUpload");
    //    }

    //    if (!ModelState.IsValid)
    //    {
    //        return View(produtoViewModel);
    //    }

    //    if (novasImagens && produtoViewModel.ImagensUpload.Count > 4)
    //    {
    //        ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
    //        return View(produtoViewModel);
    //    }

    //    var produtoDb = await _produtoRepository.ReadById(produtoViewModel.Id);

    //    if (produtoDb == null) 
    //    { 
    //        return View(produtoViewModel);
    //    }

    //    MapearProduto(produtoDb, produtoViewModel);

    //    if (!produtoViewModel.Ativo && !produtoViewModel.Desistencia)
    //    {
    //        produtoDb.DataVenda = DateTime.Now;
    //    }

    //    await _produtoService.Update(produtoDb);


    //    if (!_notificador.TemNotificacao())
    //    {
    //        if (novasImagens)
    //        {
    //            var listaProdutoImagensDb = produtoDb.ProdutoImagens;
    //            var imagensAntigasIdLista = new List<Guid>();
    //            // Deletando imagens antigas
    //            foreach (var imagem in listaProdutoImagensDb)
    //            {
    //                bool result = await DeletarArquivo(imagem.FileName);

    //                if (!result)
    //                {
    //                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");
    //                }

    //                imagensAntigasIdLista.Add(imagem.Id);


    //            }

    //            // Adicionando as imagens novas
    //            foreach (var imagem in produtoViewModel.ImagensUpload)
    //            {
    //                var imgPrefixo = Guid.NewGuid() + "_";
    //                if (!await UploadArquivo(imagem, imgPrefixo))
    //                {
    //                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

    //                    return View(produtoViewModel);
    //                }

    //                var produtoImagem = new ProdutoImagem();
    //                produtoImagem.FileName = imgPrefixo + imagem.FileName;
    //                produtoImagem.ProdutoId = produtoDb.Id;

    //                await _produtoImagemService.Create(produtoImagem);
    //            }

    //            foreach (var imagemId in imagensAntigasIdLista)
    //            {
    //                await _produtoImagemService.Delete(imagemId);
    //            }

    //        }

    //        return RedirectToAction("Index", "Home");
    //    }

    //    foreach (var error in _notificador.GetNotificacoes())
    //    {
    //        ModelState.AddModelError(error.Propriedade, error.Mensagem);
    //    }

    //    return View(produtoViewModel);

    //}

    //public async Task<IActionResult> Visualizar(Guid id)
    //{
    //    var produtoDb = await _produtoRepository.ReadById(id);

    //    if (produtoDb == null)
    //    {
    //        return View();
    //    }

    //    var produtoViewModel = _mapper.Map<GetProdutoViewModel>(produtoDb);

    //    return View(produtoViewModel);
    //}

    [HttpPost]
    public async Task<IActionResult> Editar(UpdateProdutoViewModel produtoViewModel)
    {
        bool novasImagens = produtoViewModel.ImagensUpload != null;

        if (!novasImagens)
        {
            ModelState.ClearValidationState("ImagensUpload");
            ModelState.MarkFieldValid("ImagensUpload");
        }

        if (!ModelState.IsValid)
        {
            return View(produtoViewModel);
        }

        if (novasImagens && produtoViewModel.ImagensUpload.Count > 4)
        {
            ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
            return View(produtoViewModel);
        }


        var patchProdutoViewModel = _mapper.Map<PatchProdutoViewModel>(produtoViewModel);

        patchProdutoViewModel.CondominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        patchProdutoViewModel.ImagensUploadNames = new List<string>();

        if (novasImagens)
        {
            foreach (var imagem in produtoViewModel.ImagensUpload)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(imagem, imgPrefixo))
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                    return View(produtoViewModel);
                }

                patchProdutoViewModel.ImagensUploadNames.Add(imgPrefixo + imagem.FileName);
            }

            var responseProduto = await _httpClient.GetAsync("Produto/Produto/" + produtoViewModel.Id);

            GetProdutoResponseId produtoDb;

            produtoDb = await DeserializeObjectResponse<GetProdutoResponseId>(responseProduto);


            if (!produtoDb.Success)
            {
                return View(produtoViewModel);
            }

            var listaProdutoImagensDb = produtoDb.Data.ProdutoImagemViewModels;
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
        var produtoContent = new StringContent(
        JsonSerializer.Serialize(patchProdutoViewModel),
        Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PatchAsync("Produto/Produto/", produtoContent);

        ProdutoResponse produtoResponse;

        if (!VerifyResponseErros(response))
        {
            produtoResponse = new ProdutoResponse
            {
                Success = false,
                Data = new DataProduto
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in produtoResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(produtoViewModel);
        }

        produtoResponse = await DeserializeObjectResponse<ProdutoResponse>(response);
        //

        if (!_notificador.TemNotificacao())
        {
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in _notificador.GetNotificacoes())
        {
            ModelState.AddModelError(error.Propriedade, error.Mensagem);
        }

        return View(produtoViewModel);

    }

    public async Task<IActionResult> Visualizar(Guid id)
    {
        var response = await _httpClient.GetAsync("Produto/Produto/" + id);

        GetProdutoResponseId produtoResponse;

        produtoResponse = await DeserializeObjectResponse<GetProdutoResponseId>(response);

        return View(produtoResponse.Data);
    }

    //public async Task<IActionResult> Produtos()
    //{
    //    var produtosDb = await _produtoRepository.ReadExpression(x => x.Ativo);

    //    ViewBag.Produtos = Enumerable.Empty<GetProdutoViewModel>();

    //    if (!produtosDb.Any())
    //    {
    //        return View();
    //    }

    //    var produtosViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtosDb);

    //    ViewBag.Produtos = produtosViewModel;


    //    return View();
    //}

    public async Task<IActionResult> Produtos()
    {
        var response = await _httpClient.GetAsync("Produto/Produto");

        GetAllProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<GetAllProdutoResponse>(response);

        var produtosDb = produtoResponse.Data.Where(x => x.Ativo);

        ViewBag.Produtos = Enumerable.Empty<GetProdutoViewModel>();

        if (!produtosDb.Any())
        {
            return View();
        }

        //var produtosViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtosDb);

        ViewBag.Produtos = produtosDb;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Produtos(FiltrarProdutoViewModel filtrarProdutoViewModel)
    {
        if (filtrarProdutoViewModel.Categorias == null)
        {
            return RedirectToAction("Produtos");
        }

        var produtosDb = await _produtoRepository.ReadExpression(x => filtrarProdutoViewModel.Categorias.Contains(x.Categoria) && x.Ativo == true);

        ViewBag.Produtos = Enumerable.Empty<GetProdutoViewModel>();

        if (!produtosDb.Any())
        {
            return View();
        }

        var produtosViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtosDb);

        ViewBag.Produtos = produtosViewModel;

        return View();
    }

    //public async Task<IActionResult> Deletar(Guid id)
    //{
    //    await _produtoService.Delete(id);

    //    if (!_notificador.TemNotificacao())
    //    {
    //        return RedirectToAction("MeusProdutos");
    //    }

    //    return View();
    //}

    public async Task<IActionResult> Deletar(Guid id)
    {
        await _produtoService.Delete(id);

        var response = await _httpClient.DeleteAsync("Produto/Produto/" + id);

        ProdutoResponse produtoResponse;

        if (!VerifyResponseErros(response))
        {
            produtoResponse = new ProdutoResponse
            {
                Success = false,
                Data = new DataProduto
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                }
            };

            foreach (var error in produtoResponse.Data.ResponseResult.Errors.Messages)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View();
        }

        if (!_notificador.TemNotificacao())
        {
            return RedirectToAction("MeusProdutos");
        }

        return View();
    }

    public async Task<IActionResult> Curtir(Guid id, string returnUrl)
    {
        await _produtoCurtidaService.Curtir(id, Guid.Parse(_userManager.GetUserId(this.User)));

        if (_notificador.TemNotificacao())
        {
            List<string> errors = new List<string>();

            foreach (var error in _notificador.GetNotificacoes())
            {
                errors.Add(error.Mensagem);
            }

            // não funciona
            ViewBag.Errors = errors;
        }

        if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("Produto"))
        {
            return RedirectToAction("Visualizar", new { id = id });
        }

        return LocalRedirect(returnUrl);
    }

    public async Task<IActionResult> Descurtir(Guid id, string returnUrl)
    {
        await _produtoCurtidaService.Descurtir(id, Guid.Parse(_userManager.GetUserId(this.User)));

        if (_notificador.TemNotificacao())
        {
            List<string> errors = new List<string>();

            foreach (var error in _notificador.GetNotificacoes())
            {
                errors.Add(error.Mensagem);
            }

            // não funciona
            ViewBag.Errors = errors;
        }


        if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("Produto"))
        {
            return RedirectToAction("Visualizar", new { id = id });
        }

        return LocalRedirect(returnUrl);
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

    private static void MapearProduto(Produto produto, UpdateProdutoViewModel produtoViewModel)
    {
        produto.Nome = produtoViewModel.Nome;
        produto.Descricao = produtoViewModel.Descricao;
        produto.Preco = produtoViewModel.Preco.Value;
        produto.EstadoProduto = produtoViewModel.EstadoProduto.Value;
        produto.Categoria = produtoViewModel.Categoria.Value;
        produto.Desistencia = produtoViewModel.Desistencia;
        produto.Ativo = produtoViewModel.Ativo;
    }

    #endregion

}
