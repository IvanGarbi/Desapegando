using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Models;
using Desapegando.Business.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

public class ProdutoController : MainController
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public ProdutoController(HttpClient httpClient,
                             IOptions<AppSettings> settings, 
                             IMapper mapper, 
                             INotificador notificador) : base(httpClient, settings, notificador)
    {
        _mapper = mapper;
        httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Criar()
    {
        ProdutoViewModel model = new ProdutoViewModel();
        return View(model);
    }

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

        AdicionarJWTnoHeader();

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
        AdicionarJWTnoHeader();

        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var response = await _httpClient.GetAsync("Produto/Produto/MeusProdutos/" + condominoId);

        GetMeusProdutoResponse produtosResponse;

        produtosResponse = await DeserializeObjectResponse<GetMeusProdutoResponse>(response);

        return View(produtosResponse.Data);
    }

    public async Task<IActionResult> Editar(Guid id)
    {
        AdicionarJWTnoHeader();

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

    [HttpPost]
    public async Task<IActionResult> Editar(UpdateProdutoViewModel produtoViewModel)
    {
        bool venda = false;
        int quantidadeVenda = 0;
        int quantidadeDb = 0;

        int quantidadeViewModel = produtoViewModel.Quantidade.Value;

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

        AdicionarJWTnoHeader();

        var responseProduto = await _httpClient.GetAsync("Produto/Produto/" + produtoViewModel.Id);
        GetProdutoResponseId produtoDb;
        produtoDb = await DeserializeObjectResponse<GetProdutoResponseId>(responseProduto);

        if (!produtoDb.Success)
        {
            return View(produtoViewModel);
        }

        //
        quantidadeDb = produtoDb.Data.Quantidade;

        if (produtoViewModel.Quantidade.Value == 0)
        {
            // não pode ter produto sem quantidade no banco de dados
            produtoDb.Data.Quantidade = 1;
            patchProdutoViewModel.Quantidade = 1;

            quantidadeViewModel = quantidadeDb;
        }

        if (!produtoViewModel.Ativo && !produtoViewModel.Desistencia)
        {
            quantidadeVenda = quantidadeDb - quantidadeViewModel;

            // nem todos os produtos (quantidade) foram vendidos
            if (quantidadeVenda != 0)
            {
                patchProdutoViewModel.Ativo = true;
                produtoDb.Data.Ativo = true;
            }

            //produtoDb.DataVenda = DateTime.Now;
            venda = true;

            if (quantidadeVenda == 0 && produtoViewModel.ProdutoVendido == true)
            {
                //patchProdutoViewModel.Ativo = false;
                //produtoDb.Data.Ativo = false;

                quantidadeVenda = quantidadeDb;

                // não pode ter produto sem quantidade no banco de dados
                patchProdutoViewModel.Quantidade = 1;
                produtoDb.Data.Quantidade = 1;
            }
        }

        //

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

            //var responseProduto = await _httpClient.GetAsync("Produto/Produto/" + produtoViewModel.Id);

            //GetProdutoResponseId produtoDb;

            //produtoDb = await DeserializeObjectResponse<GetProdutoResponseId>(responseProduto);




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

            if (venda && quantidadeVenda >= 0)
            {
                if (quantidadeVenda == 0)
                {
                    quantidadeVenda = quantidadeViewModel;
                }

                return RedirectToAction("Index", "Compra", new { produtoId = produtoViewModel.Id, quantidade = quantidadeVenda });
            }



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
        AdicionarJWTnoHeader();

        var response = await _httpClient.GetAsync("Produto/Produto/" + id);

        GetProdutoResponseId produtoResponse;

        produtoResponse = await DeserializeObjectResponse<GetProdutoResponseId>(response);

        return View(produtoResponse.Data);
    }

    public async Task<IActionResult> Produtos()
    {
        FiltrarProdutoViewModel model = new FiltrarProdutoViewModel();
        model.CheckBoxItems = new List<EnumModel>
        {
            new EnumModel() { EstadoProduto = EstadoProduto.Novo, IsSelected = false },
            new EnumModel() { EstadoProduto = EstadoProduto.Seminovo, IsSelected = false },
            new EnumModel() { EstadoProduto = EstadoProduto.Usado, IsSelected = false }
        };

        AdicionarJWTnoHeader();

        var response = await _httpClient.GetAsync("Produto/Produto");

        GetAllProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<GetAllProdutoResponse>(response);

        var produtosDb = produtoResponse.Data.Where(x => x.Ativo);


        if (!produtosDb.Any())
        {
            ViewBag.Produtos = Enumerable.Empty<GetProdutoViewModel>();
            
            return View(model);
        }

        ViewBag.Produtos = produtosDb;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Produtos(FiltrarProdutoViewModel filtrarProdutoViewModel)
    {
        // por algum motivo não volta o valor do front end...
        filtrarProdutoViewModel.CheckBoxItems[0].EstadoProduto = EstadoProduto.Novo;
        filtrarProdutoViewModel.CheckBoxItems[1].EstadoProduto = EstadoProduto.Seminovo;
        filtrarProdutoViewModel.CheckBoxItems[2].EstadoProduto = EstadoProduto.Usado;

        if (filtrarProdutoViewModel.Categorias == null &&
            !filtrarProdutoViewModel.CheckBoxItems.Any(x => x.IsSelected) &&
            filtrarProdutoViewModel.PrecoMinimo == null &&
            filtrarProdutoViewModel.PrecoMaximo == null)
        {
            return RedirectToAction("Produtos");
        }

        AdicionarJWTnoHeader();
        
        var response = await _httpClient.GetAsync("Produto/Produto");

        GetAllProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<GetAllProdutoResponse>(response);

        var produtosDb = produtoResponse.Data.Where(x => (filtrarProdutoViewModel.Categorias != null && filtrarProdutoViewModel.Categorias.Contains(x.Categoria)) ||
                                                                      ((filtrarProdutoViewModel.CheckBoxItems[0].EstadoProduto == x.EstadoProduto && filtrarProdutoViewModel.CheckBoxItems[0].IsSelected) ||
                                                                      (filtrarProdutoViewModel.CheckBoxItems[1].EstadoProduto == x.EstadoProduto && filtrarProdutoViewModel.CheckBoxItems[1].IsSelected) ||
                                                                      (filtrarProdutoViewModel.CheckBoxItems[2].EstadoProduto == x.EstadoProduto && filtrarProdutoViewModel.CheckBoxItems[2].IsSelected) ||
                                                                      ((filtrarProdutoViewModel.PrecoMinimo != null && filtrarProdutoViewModel.PrecoMaximo != null) &&
                                                                      (x.Preco >= filtrarProdutoViewModel.PrecoMinimo && x.Preco <= filtrarProdutoViewModel.PrecoMaximo)) ||
                                                                      (filtrarProdutoViewModel.PrecoMinimo != null && x.Preco >= filtrarProdutoViewModel.PrecoMinimo && filtrarProdutoViewModel.PrecoMaximo == null) ||
                                                                      (filtrarProdutoViewModel.PrecoMaximo != null && x.Preco <= filtrarProdutoViewModel.PrecoMaximo && filtrarProdutoViewModel.PrecoMinimo == null)) &&
                                                                      x.Ativo == true);

        if (!produtosDb.Any())
        {
            ViewBag.Produtos = Enumerable.Empty<GetProdutoViewModel>();
            
            return View();
        }

        var produtosViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtosDb);

        ViewBag.Produtos = produtosViewModel;

        return View();
    }

    public async Task<IActionResult> Deletar(Guid id)
    {
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
        CurtidaViewModel curtidaViewModel = new CurtidaViewModel
        {
            CondominoId = Guid.Parse(User.FindFirst("sub")?.Value),
            ProdutoId = id
        };

        AdicionarJWTnoHeader();

        var curtidaContent = new StringContent(
        JsonSerializer.Serialize(curtidaViewModel),
        Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PostAsync("Produto/Produto/Curtir/", curtidaContent);

        ProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<ProdutoResponse>(response);

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

            List<string> errors = new List<string>();

            foreach (var error in produtoResponse.Data.ResponseResult.Errors.Messages)
            {
                errors.Add(error);
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
        DescurtidaViewModel descurtidaViewModel = new DescurtidaViewModel
        {
            CondominoId = Guid.Parse(User.FindFirst("sub")?.Value),
            ProdutoId = id
        };

        AdicionarJWTnoHeader();

        var descurtidaContent = new StringContent(
                JsonSerializer.Serialize(descurtidaViewModel),
                Encoding.UTF8,
                "application/json");

        var response = await _httpClient.PostAsync("Produto/Produto/Descurtir/", descurtidaContent);

        ProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<ProdutoResponse>(response);

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

            List<string> errors = new List<string>();

            foreach (var error in produtoResponse.Data.ResponseResult.Errors.Messages)
            {
                errors.Add(error);
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

    [HttpPost]
    public async Task<IActionResult> RemoverProduto(Guid id, string motivo)
    {
        RemoverProdutoViewModel removerProdutoViewModel = new RemoverProdutoViewModel
        {
            ProdutoId = id,
            Motivo = motivo
        };

        AdicionarJWTnoHeader();

        var removerProdutoContent = new StringContent(
                        JsonSerializer.Serialize(removerProdutoViewModel),
                        Encoding.UTF8,
                        "application/json");

        var response = await _httpClient.PostAsync("Produto/Produto/RemoverProduto/", removerProdutoContent);

        ProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<ProdutoResponse>(response);

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

            List<string> errors = new List<string>();

            foreach (var error in produtoResponse.Data.ResponseResult.Errors.Messages)
            {
                errors.Add(error);
            }

            //return Json(HttpStatusCode.NotFound);
            return Json(new { status = HttpStatusCode.NotFound, erro = errors.FirstOrDefault()});
        }


        return Json(new { status = HttpStatusCode.OK });
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
