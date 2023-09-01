using AutoMapper;
using Desapegando.API.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Services;
using Desapegando.Business.Validations;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.API.Controllers
{
    [Route("Produto/[controller]")]
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProdutoViewModel>>> Get()
        {
            return Response(_mapper.Map<IEnumerable<GetProdutoViewModel>>(await _produtoRepository.Read()));
        }

        [HttpGet("MeusProdutos/{condominoId:guid}")]
        public async Task<ActionResult<IEnumerable<GetProdutoViewModel>>> GetMeusProdutos(Guid condominoId)
        {
            return Response(_mapper.Map<IEnumerable<GetProdutoViewModel>>(await _produtoRepository.ReadExpression(x => x.CondominoId == condominoId && x.Ativo == true)));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetProdutoViewModel>> Get(Guid id)
        {
            return Response(_mapper.Map<GetProdutoViewModel>(await _produtoRepository.ReadById(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProdutoViewModel postProdutoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Response(postProdutoViewModel);
            }

            //var imagensUploadNames = new List<string>();
            //imagensUploadNames.AddRange(postProdutoViewModel.ImagensUploadNames);

            if (postProdutoViewModel.ImagensUploadNames.Any() && postProdutoViewModel.ImagensUploadNames.Count > 4)
            {
                ModelState.AddModelError("ImagensUploadNames", "Só é possível adicionar no máximo 4 imagens.");
                return Response(postProdutoViewModel);
            }

            var produto = _mapper.Map<Produto>(postProdutoViewModel);

            await _produtoService.Create(produto);



            if (!_notificador.TemNotificacao())
            {
                foreach (var imagemNome in postProdutoViewModel.ImagensUploadNames)
                {
                    var produtoImagem = new ProdutoImagem();
                    produtoImagem.FileName = imagemNome;
                    produtoImagem.ProdutoId = produto.Id;

                    await _produtoImagemService.Create(produtoImagem);
                }
                
                //foreach (var imagem in postProdutoViewModel.ImagensUpload)
                //{
                //    var imgPrefixo = Guid.NewGuid() + "_";
                //    //if (!await UploadArquivo(imagem, imgPrefixo))
                //    //{
                //    //    await _produtoService.Delete(produto.Id);

                //    //    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                //    //    return View(postProdutoViewModel);
                //    //}

                //    var produtoImagem = new ProdutoImagem();
                //    produtoImagem.FileName = imgPrefixo + imagem.FileName;
                //    produtoImagem.ProdutoId = produto.Id;

                //    await _produtoImagemService.Create(produtoImagem);

                //}

                return Response();
            }

            foreach (var error in _notificador.GetNotificacoes())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return Response(postProdutoViewModel);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] PatchProdutoViewModel produtoViewModel)
        {
            //bool novasImagens = produtoViewModel.ImagensUploadNames != null;
            bool novasImagens = produtoViewModel.ImagensUploadNames.Any();

            if (!novasImagens)
            {
                ModelState.ClearValidationState("ImagensUpload");
                ModelState.MarkFieldValid("ImagensUpload");
            }

            if (!ModelState.IsValid)
            {
                return Response(produtoViewModel);
            }

            if (novasImagens && produtoViewModel.ImagensUploadNames.Count > 4)
            {
                ModelState.AddModelError("ImagensUploadNames", "Só é possível adicionar no máximo 4 imagens.");
                return Response(produtoViewModel);
            }

            var produtoDb = await _produtoRepository.ReadById(produtoViewModel.Id);

            if (produtoDb == null)
            {
                ModelState.AddModelError(string.Empty, "Produto não encontrado.");
                return Response(produtoViewModel);
            }

            MapearProduto(produtoDb, produtoViewModel);

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
                    // Deletando imagens antigas
                    foreach (var imagem in listaProdutoImagensDb)
                    {
                        await _produtoImagemService.Delete(imagem.Id);
                    }

                    // Adicionando as imagens novas
                    foreach (var imagemNome in produtoViewModel.ImagensUploadNames)
                    {
                        var produtoImagem = new ProdutoImagem();
                        produtoImagem.FileName = imagemNome;
                        produtoImagem.ProdutoId = produtoDb.Id;

                        await _produtoImagemService.Create(produtoImagem);
                    }
                }

                return Response();
            }

            foreach (var error in _notificador.GetNotificacoes())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return Response(produtoViewModel);

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _produtoService.Delete(id);

            if (_notificador.TemNotificacao())
            {
                foreach (var error in _notificador.GetNotificacoes())
                {
                    ModelState.AddModelError(error.Propriedade, error.Mensagem);
                };

                return Response(ModelState);
            }

            return Response();
        }

        [HttpPost("Curtir")]
        public async Task<IActionResult> Curtir([FromBody] CurtidaViewModel curtidaViewModel)
        {
            await _produtoCurtidaService.Curtir(curtidaViewModel.ProdutoId, curtidaViewModel.CondominoId);

            if (_notificador.TemNotificacao())
            {
                List<string> errors = new List<string>();

                foreach (var error in _notificador.GetNotificacoes())
                {
                    errors.Add(error.Mensagem);
                }
            }

            return Response();
        }

        [HttpPost("Descurtir")]
        public async Task<IActionResult> Descurtir(DescurtidaViewModel descurtidaViewModel)
        {
            await _produtoCurtidaService.Descurtir(descurtidaViewModel.ProdutoId, descurtidaViewModel.CondominoId);

            if (_notificador.TemNotificacao())
            {
                List<string> errors = new List<string>();

                foreach (var error in _notificador.GetNotificacoes())
                {
                    errors.Add(error.Mensagem);
                }
            }

            return Response();
        }

        #region Métodos Privados
        private static void MapearProduto(Produto produto, PatchProdutoViewModel produtoViewModel)
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
}
