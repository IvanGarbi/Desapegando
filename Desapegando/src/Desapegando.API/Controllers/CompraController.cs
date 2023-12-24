using AutoMapper;
using Desapegando.API.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Services;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.API.Controllers
{
    [Route("[controller]")]
    public class CompraController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICondominoRepository _condominoRepository;
        private readonly IProdutoService _produtoService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICompraRepository _compraRepository;
        private readonly ICompraService _compraService;
        private readonly IMapper _mapper;

        public CompraController(IProdutoRepository produtoRepository,
                                IProdutoService produtoService,
                                IMapper mapper,
                                ICondominoRepository condominoRepository,
                                UserManager<IdentityUser> userManager,
                                ICompraRepository compraRepository,
                                ICompraService compraService,
                                INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
            _userManager = userManager;
            _condominoRepository = condominoRepository;
            _compraRepository = compraRepository;
            _compraService = compraService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCompraViewModel>>> Get()
        {
            return Response(_mapper.Map<IEnumerable<GetCompraViewModel>>(await _compraRepository.Read()));
        }

        [HttpGet("MinhasCompras/{condominoId:guid}")]
        public async Task<ActionResult<IEnumerable<GetCompraViewModel>>> GetMinhasCompras(Guid condominoId)
        {
            return Response(_mapper.Map<IEnumerable<GetCompraViewModel>>(await _compraRepository.ReadExpression(x => x.CondominoId == condominoId)));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCompraViewModel>> Get(Guid id)
        {
            return Response(_mapper.Map<GetCompraViewModel>(await _compraRepository.ReadById(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostCompraViewModel postCompraViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Response(postCompraViewModel);
            }

            var compra = _mapper.Map<Compra>(postCompraViewModel);

            compra.DataVenda = DateTime.Now;

            await _compraService.Create(compra);

            if (!_notificador.TemNotificacao())
            {
                //return Response();
                return ResponseCeated();
            }

            foreach (var error in _notificador.GetNotificacoes())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return Response(postCompraViewModel);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] PatchCompraViewModel compraViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Response(compraViewModel);
            }

            var compraDb = await _compraRepository.ReadById(compraViewModel.Id);

            if (compraDb == null)
            {
                ModelState.AddModelError(string.Empty, "Compra não encontrado.");
                return Response(compraViewModel);
            }

            MapearCompra(compraDb, compraViewModel);

            await _compraService.Update(compraDb);

            if (!_notificador.TemNotificacao())
            {
                return Response();
            }

            foreach (var error in _notificador.GetNotificacoes())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return Response(compraViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _compraService.Delete(id);

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

        #region Métodos Privados
        private static void MapearCompra(Compra compra, PatchCompraViewModel compraViewModel)
        {
            compra.CondominoId = compraViewModel.CondominoId;
            compra.ProdutoId = compraViewModel.ProdutoId;
        }

        #endregion
    }
}
