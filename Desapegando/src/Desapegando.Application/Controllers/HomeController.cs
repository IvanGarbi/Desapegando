using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Desapegando.Business.Interfaces.Repository;
using AutoMapper;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Authorization;

namespace Desapegando.Application.Controllers;

public class HomeController : MainController
{
    private readonly ICampanhaRepository _campanhaRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;

    public HomeController(IMapper mapper, 
                          IProdutoRepository produtoRepository, 
                          ICampanhaRepository campanhaRepository,
                          INotificador notificador) : base(notificador)
    {
        _mapper = mapper;
        _produtoRepository = produtoRepository;
        _campanhaRepository = campanhaRepository;

    }

    public async Task<IActionResult> Index()
    {
        var produtos = await _produtoRepository.Read();
        var campanhas = await _campanhaRepository.Read();

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