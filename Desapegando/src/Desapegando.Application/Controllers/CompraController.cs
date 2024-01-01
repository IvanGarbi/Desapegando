using AutoMapper;
using Desapegando.Application.Services.MVC;
using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers
{
    public class CompraController : MainController
    {
        private readonly IMapper _mapper;
        private readonly CondominoService _condominoService;
        private readonly ICompraService _compraService;
        private readonly IProdutoService _produtoService;

        public CompraController(CondominoService condominoService,
                                ICompraService compraService,
                                IProdutoService produtoService,
                                IMapper mapper)
        {
            _mapper = mapper;
            _compraService = compraService;
            _condominoService = condominoService;
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index(Guid produtoId, int quantidade)
        {

            var response = await _condominoService._httpClient.GetAsync("Condomino");

            GetCondominoCompraResponse condominoResponse;

            condominoResponse = await DeserializeObjectResponse<GetCondominoCompraResponse>(response);

            var condominoComprasViewModel = condominoResponse.Data.Where(x => x.Ativo == true && x.Id != Guid.Parse(User.FindFirst("sub")?.Value));

            ViewBag.Quantidade = quantidade;

            condominoComprasViewModel.ToList().ForEach(x => x.ProdutoId = produtoId);

            return View(condominoComprasViewModel);
        }

        public async Task<IActionResult> Historico()
        {
            var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

            var response = await _compraService.GetMinhasCompras(condominoId);

            //GetCompraResponse compraResponse;

            //compraResponse = await DeserializeObjectResponse<GetCompraResponse>(response);

            var meusProdutosDbViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(response.Data.Select(x => x.Produto));

            return View(meusProdutosDbViewModel);
        }

        //[HttpPost]
        public async Task<IActionResult> AdicionarComprador(Guid id, int quantidade, Guid produtoId, int quantidadeTotal)
        {
            if (id == Guid.Empty || quantidade == 0 || produtoId == Guid.Empty || quantidadeTotal == 0)
            {
                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            }

            if (quantidadeTotal < quantidade)
            {
                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            }

            var responseProdutoById = await _produtoService.GetProduto(id);

            var produto = responseProdutoById;

            var responseCondominoById = await _condominoService._httpClient.GetAsync("Condomino/" + id);

            GetCondominoResponseId condominoResponse;

            condominoResponse = await DeserializeObjectResponse<GetCondominoResponseId>(responseCondominoById);

            var condomino = condominoResponse;


            if (condomino == null || produto == null)
            {
                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            }

            try
            {

                for (int i = 0; i < quantidade; i++)
                {
                    CompraViewModel compraViewModel = new CompraViewModel
                    {
                        CondominoId = id,
                        ProdutoId = produtoId,
                        DataVenda = DateTime.Now,
                    };

                    //var compraContent = new StringContent(
                    //        JsonSerializer.Serialize(compraViewModel),
                    //        Encoding.UTF8,
                    //        "application/json");

                    var response = await _compraService.CriarCompra(compraViewModel);

                    CompraResponse compraResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            }

            var qtd = quantidadeTotal - quantidade;

            if (qtd > 0)
            {
                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = qtd });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
