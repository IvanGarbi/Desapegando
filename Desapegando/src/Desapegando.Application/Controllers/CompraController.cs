using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.Services.MVC;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers
{
    public class CompraController : MainController
    {
        private readonly IMapper _mapper;
        private readonly CondominoService _condominoService;
        private readonly CompraService _compraService;
        private readonly ProdutoService _produtoService;

        public CompraController(CondominoService condominoService,
                                CompraService compraService,
                                ProdutoService produtoService,
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

            var response = await _compraService._httpClient.GetAsync("Compra/MinhasCompras/" + condominoId);

            GetCompraResponse compraResponse;

            compraResponse = await DeserializeObjectResponse<GetCompraResponse>(response);

            var meusProdutosDbViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(compraResponse.Data.Select(x => x.Produto));

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

            var responseProdutoById = await _produtoService._httpClient.GetAsync("Produto/" + id);

            GetProdutoResponseId produtoResponse;

            produtoResponse = await DeserializeObjectResponse<GetProdutoResponseId>(responseProdutoById);

            var produto = produtoResponse;

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
                    Compra compra = new Compra
                    {
                        CondominoId = id,
                        ProdutoId = produtoId,
                        DataVenda = DateTime.Now,
                    };

                    var compraContent = new StringContent(
                            JsonSerializer.Serialize(compra),
                            Encoding.UTF8,
                            "application/json");

                    var response = await _compraService._httpClient.PostAsync("Compra/", compraContent);

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
