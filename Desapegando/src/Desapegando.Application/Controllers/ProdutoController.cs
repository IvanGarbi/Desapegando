using AutoMapper;
using Desapegando.Application.Services.MVC;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Models;
using Desapegando.Business.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Desapegando.Application.Controllers;

public class ProdutoController : MainController
{
    private readonly IMapper _mapper;
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService,
                             IMapper mapper)
    {
        _mapper = mapper;
        _produtoService = produtoService;
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

        var response = await _produtoService.CriarProduto(postProdutoViewModel);

        if (ResponsePossuiErros(response))
        {
            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(produtoViewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> MeusProdutos()
    {
        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var teste = await _produtoService.GetMeusProdutos(condominoId);

        return View(teste.Data);
    }

    public async Task<IActionResult> Editar(Guid id)
    {
        var response = await _produtoService.GetProduto(id);

        if (!response.Success)
        {
            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View();
        }

        var produto = _mapper.Map<Produto>(response.Data);

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

        var responseProduto = await _produtoService.GetProduto(produtoViewModel.Id);

        GetProdutoResponseId produtoDb = responseProduto;

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

        var response = await _produtoService.UpdateProduto(patchProdutoViewModel);

        if (ResponsePossuiErros(response))
        {
            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(produtoViewModel);
        }

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

    public async Task<IActionResult> Visualizar(Guid id)
    {
        var response = await _produtoService.GetProduto(id);

        return View(response.Data);
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

        var response = await _produtoService.GetProdutos();

        var produtosDb = response.Data.Where(x => x.Ativo);

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

        var response = await _produtoService.GetProdutos();

        var produtosDb = response.Data.Where(x => (filtrarProdutoViewModel.Categorias != null && filtrarProdutoViewModel.Categorias.Contains(x.Categoria)) ||
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
        var response = await _produtoService.DeletarProduto(id);

        if (ResponsePossuiErros(response))
        {
            return View();
        }

        return RedirectToAction("MeusProdutos");
    }

    public async Task<IActionResult> Curtir(Guid id, string returnUrl)
    {
        CurtidaViewModel curtidaViewModel = new CurtidaViewModel
        {
            CondominoId = Guid.Parse(User.FindFirst("sub")?.Value),
            ProdutoId = id
        };

        var response = await _produtoService.CurtirProduto(curtidaViewModel);

        if (ResponsePossuiErros(response))
        {
            return View();
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

        var response = await _produtoService.DescurtirProduto(descurtidaViewModel);
        if (ResponsePossuiErros(response))
        {
            return View();
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

        var response = await _produtoService.RemoverProduto(removerProdutoViewModel);

        if (response != null && response.Errors.Messages.Any())
        {
            List<string> errors = new List<string>();

            foreach (var error in response.Errors.Messages)
            {
                errors.Add(error);
            }

            return Json(new { status = HttpStatusCode.NotFound, erro = errors.FirstOrDefault() });
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
