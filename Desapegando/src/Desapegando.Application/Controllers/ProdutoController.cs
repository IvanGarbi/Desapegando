using AutoMapper;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers
{
    public class ProdutoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IProdutoCurtidaService _produtoCurtidaService;
        private readonly IProdutoImagemService _produtoImagemService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ProdutoController(IProdutoRepository produtoRepository,
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

            var produto = _mapper.Map<Produto>(produtoViewModel);

            produto.CondominoId = Guid.Parse(_userManager.GetUserId(this.User));

            await _produtoService.Create(produto);

            if (!_notificador.TemNotificacao())
            {
                foreach (var imagem in produtoViewModel.ImagensUpload)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(imagem, imgPrefixo))
                    {
                        await _produtoService.Delete(produto.Id);

                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                        return View(produtoViewModel);
                    }

                    var produtoImagem = new ProdutoImagem();
                    produtoImagem.FileName = imgPrefixo + imagem.FileName;
                    produtoImagem.ProdutoId = produto.Id;

                    await _produtoImagemService.Create(produtoImagem);

                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in _notificador.GetNotifications())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return View(produtoViewModel);
        }

        public async Task<IActionResult> MeusProdutos()
        {
            var todosProdutoDb = await _produtoRepository.Read();

            var meusProdutosDb = todosProdutoDb.Where(x => x.CondominoId == Guid.Parse(_userManager.GetUserId(this.User)) && x.Ativo == true);

            if (meusProdutosDb == null)
            {
                return View();
            }

            var meusProdutosDbViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(meusProdutosDb);

            return View(meusProdutosDbViewModel);
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            var produtoDb = await _produtoRepository.ReadById(id);

            if (produtoDb == null)
            {
                return View();
            }

            var produtoViewModel = _mapper.Map<UpdateProdutoViewModel>(produtoDb);

            return View(produtoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UpdateProdutoViewModel produtoViewModel)
        {
            var novasImagens = produtoViewModel.ImagensUpload != null;

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

            var produtoDb = await _produtoRepository.ReadById(produtoViewModel.Id);

            if (produtoDb == null) 
            { 
                return View(produtoViewModel);
            }
            
            produtoDb.Nome = produtoViewModel.Nome;
            produtoDb.Descricao = produtoViewModel.Descricao;
            produtoDb.Preco = produtoViewModel.Preco.Value;
            produtoDb.EstadoProduto = produtoViewModel.EstadoProduto.Value;
            produtoDb.Categoria = produtoViewModel.Categoria.Value;
            produtoDb.Desistencia = produtoViewModel.Desistencia;
            produtoDb.Ativo = produtoViewModel.Ativo;

            if (!produtoViewModel.Ativo && !produtoViewModel.Desistencia)
            {
                produtoDb.DataVenda = DateTime.Now;
            }

            await _produtoService.Update(produtoDb);


            if (!_notificador.TemNotificacao())
            {
                if (novasImagens)
                {
                    var listaProdutoImagensDb = produtoDb.ProdutoImagens;
                    var imagensAntigasIdLista = new List<Guid>();
                    // Deletando imagens antigas
                    foreach (var imagem in listaProdutoImagensDb)
                    {
                        bool result = await DeletarArquivo(imagem.FileName);

                        if (!result)
                        {
                            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");
                        }

                        imagensAntigasIdLista.Add(imagem.Id);


                    }

                    // Adicionando as imagens novas
                    foreach (var imagem in produtoViewModel.ImagensUpload)
                    {
                        var imgPrefixo = Guid.NewGuid() + "_";
                        if (!await UploadArquivo(imagem, imgPrefixo))
                        {
                            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                            return View(produtoViewModel);
                        }

                        var produtoImagem = new ProdutoImagem();
                        produtoImagem.FileName = imgPrefixo + imagem.FileName;
                        produtoImagem.ProdutoId = produtoDb.Id;

                        await _produtoImagemService.Create(produtoImagem);
                    }

                    foreach (var imagemId in imagensAntigasIdLista)
                    {
                        await _produtoImagemService.Delete(imagemId);
                    }

                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in _notificador.GetNotifications())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return View(produtoViewModel);

        }

        public async Task<IActionResult> Visualizar(Guid id)
        {
            var produtoDb = await _produtoRepository.ReadById(id);

            if (produtoDb == null)
            {
                return View();
            }

            var produtoViewModel = _mapper.Map<GetProdutoViewModel>(produtoDb);

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Produtos()
        {
            var produtosDb = await _produtoRepository.Read();

            produtosDb = produtosDb.Where(x => x.Ativo);

            ViewBag.produtos = Enumerable.Empty<GetProdutoViewModel>();

            if (!produtosDb.Any())
            {
                return View();
            }

            var produtosViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtosDb);

            ViewBag.produtos = produtosViewModel;


            return View();
            //return View(produtosViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Produtos(FiltrarProdutoViewModel filtrarProdutoViewModel)
        {
            if (filtrarProdutoViewModel.Categorias == null)
            {
                return RedirectToAction("Produtos");
            }

            var produtosDb = await _produtoRepository.ReadExpression(x => filtrarProdutoViewModel.Categorias.Contains(x.Categoria) && x.Ativo == true);

            ViewBag.produtos = Enumerable.Empty<GetProdutoViewModel>();

            if (!produtosDb.Any())
            {
                return View();
            }

            var produtosViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtosDb);

            ViewBag.produtos = produtosViewModel;

            return View();
            //return View(produtosViewModel);
        }

        public async Task<IActionResult> Deletar(Guid id)
        {
            await _produtoService.Delete(id);

            if (!_notificador.TemNotificacao())
            {
                return RedirectToAction("MeusProdutos");
            }

            return View();
        }

        public async Task<IActionResult> Curtir(Guid id)
        {
            await _produtoCurtidaService.Curtir(id, Guid.Parse(_userManager.GetUserId(this.User)));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Descurtir(Guid id)
        {
            await _produtoCurtidaService.Descurtir(id, Guid.Parse(_userManager.GetUserId(this.User)));

            return RedirectToAction("Index", "Home");
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
}
