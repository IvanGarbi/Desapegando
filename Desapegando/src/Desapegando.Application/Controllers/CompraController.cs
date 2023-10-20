using AutoMapper;
using Desapegando.Application.Extensions;
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
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CompraController(HttpClient httpClient,
                                 IOptions<AppSettings> settings,
                                 IMapper mapper,
                                 INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(Guid produtoId, int quantidade)
        {
            var response = await _httpClient.GetAsync("Condomino/Condomino");

            GetCondominoCompraResponse condominoResponse;

            condominoResponse = await DeserializeObjectResponse<GetCondominoCompraResponse>(response);

            var condominoComprasViewModel = condominoResponse.Data.Where(x => x.Ativo == true && x.Id != Guid.Parse(User.FindFirst("sub")?.Value));

            ViewBag.Quantidade = quantidade;

            condominoComprasViewModel.ToList().ForEach(x => x.ProdutoId = produtoId);

            return View(condominoComprasViewModel);





            //var condominos = await _condominoRepository.ReadWithExpressionList(x => x.Ativo == true && x.Id != Guid.Parse(User.Claims.First().Value));

            //var condominoComprasViewModel = _mapper.Map<IEnumerable<CondominoComprasViewModel>>(condominos);

            //ViewBag.Quantidade = quantidade;

            //condominoComprasViewModel.ToList().ForEach(x => x.ProdutoId = produtoId);

            ////return View(_mapper.Map<IEnumerable<CondominoViewModel>>(condominos));
            //return View(condominoComprasViewModel);
        }

        public async Task<IActionResult> Historico()
        {
            var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

            var response = await _httpClient.GetAsync("Compra/Compra/MinhasCompras/" + condominoId);

            GetCompraResponse compraResponse;

            compraResponse = await DeserializeObjectResponse<GetCompraResponse>(response);

            var meusProdutosDbViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(compraResponse.Data.Select(x => x.Produto));

            return View(meusProdutosDbViewModel);

            //var comprasHistorico = await _compraRepository.ReadExpression(x => x.CondominoId == Guid.Parse(User.Claims.First().Value));

            //var meusProdutosDbViewModel = _mapper.Map<IEnumerable<GetProdutoViewModel>>(comprasHistorico.Select(x => x.Produto));

            //return View(meusProdutosDbViewModel);
        }

        //[HttpPost]
        public async Task<IActionResult> AdicionarComprador(Guid id, int quantidade, Guid produtoId, int quantidadeTotal)
        {
            if (id == Guid.Empty || quantidade == 0 || produtoId == Guid.Empty || quantidadeTotal == 0)
            {
                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });

                //return Json(HttpStatusCode.NotFound);
            }

            if (quantidadeTotal < quantidade)
            {
                //return Json(HttpStatusCode.BadRequest); // 400 significa que a pessoa selecionou mais do que a quantidade existente para isso

                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            }

            //var produto = await _produtoRepository.ReadById(produtoId);

            var responseProdutoById = await _httpClient.GetAsync("Produto/Produto/" + id);

            GetProdutoResponseId produtoResponse;

            produtoResponse = await DeserializeObjectResponse<GetProdutoResponseId>(responseProdutoById);

            var produto = produtoResponse;




            //var condomino = await _condominoRepository.ReadById(id);


            var responseCondominoById = await _httpClient.GetAsync("Condomino/Condomino/" + id);

            GetCondominoResponseId condominoResponse;

            condominoResponse = await DeserializeObjectResponse<GetCondominoResponseId>(responseCondominoById);

            var condomino = condominoResponse;


            if (condomino == null || produto == null)
            {
                //return RedirectToAction("Index", "Compras");
                //return Json(HttpStatusCode.NotFound);
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

                    var response = await _httpClient.PostAsync("Compra/Compra/", compraContent);

                    CompraResponse compraResponse;
                }

                //Compra compra = new Compra
                //{
                //    CondominoId = id,
                //    ProdutoId = produtoId,
                //    DataVenda = DateTime.Now,
                //};

                //await _compraService.Create(compra);


                //var compraContent = new StringContent(
                //        JsonSerializer.Serialize(compra),
                //        Encoding.UTF8,
                //        "application/json");

                //var response = await _httpClient.PostAsync("Compra/Compra/", compraContent);

                //CompraResponse compraResponse;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return Json(HttpStatusCode.NotFound);

                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            }

            //try
            //{
            //    produto.Quantidade -= quantidade;

            //    await _produtoRepository.Update(produto);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    //return Json(HttpStatusCode.NotFound);

            //    return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = quantidadeTotal });
            //}

            var qtd = quantidadeTotal - quantidade;

            if (qtd > 0)
            {
                return RedirectToAction("Index", "Compra", new { produtoId = produtoId, quantidade = qtd });
            }

            return RedirectToAction("Index", "Home");

            //return Json(HttpStatusCode.OK);
        }
    }
}
