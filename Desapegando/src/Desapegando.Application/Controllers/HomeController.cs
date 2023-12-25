using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AutoMapper;
using Desapegando.Business.Models;
using System.Text.Json;
using Desapegando.Application.Services.MVC;
namespace Desapegando.Application.Controllers;

public class HomeController : MainController
{
    private readonly IMapper _mapper;
    private readonly IProdutoService _produtoService;
    private readonly ICampanhaService _campanhaService;

    public HomeController(IMapper mapper,
                          IProdutoService produtoService,
                          ICampanhaService campanhaService)
    {
        _mapper = mapper;
        _produtoService = produtoService;
        _campanhaService = campanhaService;
    }

    public async Task<IActionResult> Index()
    {
        var produtoResponse = await _produtoService.GetProdutos();

        var campanhaResponse = await _campanhaService.GetCampanhas();

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

    protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
    }
}