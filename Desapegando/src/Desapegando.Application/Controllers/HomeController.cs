using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AutoMapper;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.Extensions.Options;
using Desapegando.Application.Extensions;
using Desapegando.Business.Models;
namespace Desapegando.Application.Controllers;

public class HomeController : MainController
{
    private readonly IMapper _mapper;

    public HomeController(IMapper mapper,
                          HttpClient httpClient,
                          IOptions<AppSettings> settings,
                          INotificador notificador) : base(httpClient, settings, notificador)
    {
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        AdicionarJWTnoHeader();

        var responseProduto = await _httpClient.GetAsync("Produto/Produto");
        
        GetAllProdutoResponse produtoResponse;

        produtoResponse = await DeserializeObjectResponse<GetAllProdutoResponse>(responseProduto);

        var responseCampanha = await _httpClient.GetAsync("Campanha/Campanha");

        GetAllCampanhaResponse campanhaResponse;

        campanhaResponse = await DeserializeObjectResponse<GetAllCampanhaResponse>(responseCampanha);

        var produtos = _mapper.Map<IEnumerable<Produto>>(produtoResponse.Data);
        var campanhas = _mapper.Map<IEnumerable<Campanha>>(campanhaResponse.Data);

        produtos = produtos.Where(x => x.Ativo);
        campanhas = campanhas.Where(x => x.Ativo);


        var produtosViewModels = _mapper.Map<IEnumerable<GetProdutoViewModel>>(produtos);
        var campanhasViewModels = _mapper.Map<IEnumerable<GetCampanhaViewModel>>(campanhas);

        var getHomeViewModel = new GetHomeViewModel();

        getHomeViewModel.GetProdutoViewModels = produtosViewModels.ToList();
        getHomeViewModel.GetCampanhaViewModels = campanhasViewModels.ToList();

        return View(getHomeViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}