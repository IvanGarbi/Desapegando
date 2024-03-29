﻿using AutoMapper;
using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Desapegando.Business.Models;
using Desapegando.Application.Services.MVC;

namespace Desapegando.Application.Controllers;

public class AdministradorController : MainController
{
    private readonly IMapper _mapper;
    private readonly CondominoService _condominoService;
    private readonly ICampanhaService _campanhaService;
    private readonly IAdministradorService _administradorService;
    private readonly IProdutoService _produtoService;
    private readonly ICompraService _compraService;

    public AdministradorController(CondominoService condominoService,
                                   ICompraService compraService,
                                   IProdutoService produtoService,
                                   ICampanhaService campanhaService,
                                   IAdministradorService administradorService,
                                   IMapper mapper)
    {
        _mapper = mapper;
        _condominoService = condominoService;
        _produtoService = produtoService;
        _compraService = compraService;
        _administradorService = administradorService;
        _campanhaService = campanhaService;
    }

    public async Task<IActionResult> NovosCondominos()
    {
        var response = await _condominoService._httpClient.GetAsync("Condomino");

        GetAllCondominoResponse condominoResponse;

        condominoResponse = await DeserializeObjectResponse<GetAllCondominoResponse>(response);

        var condominosDb = condominoResponse.Data.Where(x => x.Ativo == false);

        return View(_mapper.Map<IEnumerable<CondominoInativoViewModel>>(condominosDb));
    }

    public async Task<IActionResult> AtivarCondomino(Guid id)
    {
        var response = await _administradorService.AtivarCondomino(id);

        if (ResponsePossuiErros(response))
        {
            ViewBag.Error = "Ocorreu um erro.";

            return RedirectToAction("NovosCondominos", "Administrador");
        }

        return RedirectToAction("NovosCondominos", "Administrador");
    }

    [HttpPost]
    public async Task<IActionResult> ExcluirCondomino(Guid id)
    {
        var response = await _administradorService.ExcluirCondomino(id);

        if (ResponsePossuiErros(response))
        {
            return Json(new { status = HttpStatusCode.NotFound, erro = response.Errors.Messages.FirstOrDefault() });
        }

        return Json(new { status = HttpStatusCode.OK });
    }

    public async Task<IActionResult> Dashboard()
    {
        var responseCondomino = await _condominoService._httpClient.GetAsync("Condomino");

        GetAllCondominoResponse condominoResponse;

        condominoResponse = await DeserializeObjectResponse<GetAllCondominoResponse>(responseCondomino);

        var produtoResponse = await _produtoService.GetProdutos();

        var campanhaResponse = await _campanhaService.GetCampanhas();
        
        var compraResponse = await _compraService.GetCompras();

        var produtos = _mapper.Map<IEnumerable<Produto>>(produtoResponse.Data);
        var campanhas = _mapper.Map<IEnumerable<Campanha>>(campanhaResponse.Data);
        var condominos = _mapper.Map<IEnumerable<Condomino>>(condominoResponse.Data);
        var compras = _mapper.Map<IEnumerable<Compra>>(compraResponse.Data);

        // primeiros resultados
        var produtosDesistidos = produtos.Where(x => x.Desistencia == true);
        var produtosVendidos = compras;
        var produtosDisponiveis = produtos.Where(x => x.Ativo == true).Sum(y => y.Quantidade);

        // Novos condôminos 7 dias
        var novosCondominos7Dias = condominos.Where(x => x.Ativo == true && x.DataRegistro >= DateTime.Now.AddMonths(-2)).ToArray(); // pegar de 2 meses para facilitar, pois a iteração da data é quem define
        var datas = new List<DateTime>();
        for (int i = 6; i >= 0; i--)
        {
            datas.Add(DateTime.Now.AddDays(-i));
            datas.Add(DateTime.Now.AddDays(-i).AddMonths(-1));
        }

        List<NovosCondominos7DiasViewModel> listanovosCondominos7DiasViewModel = new List<NovosCondominos7DiasViewModel>();
        foreach (var data in datas)
        {
            NovosCondominos7DiasViewModel novosCondominos7DiasViewModel = new NovosCondominos7DiasViewModel
            {
                DataRegistro = data,
                Quantidade = novosCondominos7Dias.Where(x => x.DataRegistro.Day == data.Day && x.DataRegistro.Month == data.Month) == null ? 0 : novosCondominos7Dias.Where(x => x.DataRegistro.Day == data.Day && x.DataRegistro.Month == data.Month).Count()
            };

            listanovosCondominos7DiasViewModel.Add(novosCondominos7DiasViewModel);
        }

        listanovosCondominos7DiasViewModel = listanovosCondominos7DiasViewModel.OrderBy(x => x.DataRegistro).ToList();

        // total vendas 7 dias
        var totalVendas7Dias = produtosVendidos.Where(x => x.DataVenda >= DateTime.Now.AddDays(-8));
        datas.Clear();
        for (int i = 6; i >= 0; i--)
        {
            datas.Add(DateTime.Now.AddDays(-i));
        }
        List<Vendas7DiasViewModel> listaVendas7DiasViewModel = new List<Vendas7DiasViewModel>();
        foreach (var data in datas)
        {
            Vendas7DiasViewModel vendas7DiasViewModel = new Vendas7DiasViewModel
            {
                DataVenda = data,
                Quantidade = totalVendas7Dias.Where(x => x.DataVenda.Value.Day == data.Day) == null ? 0 : totalVendas7Dias.Where(x => x.DataVenda.Value.Day == data.Day).Count()
            };

            listaVendas7DiasViewModel.Add(vendas7DiasViewModel);
        }

        // total produtos 7 dias
        var totalProdutosVendidos7Dias = totalVendas7Dias;
        var totalProdutosDesistidos7Dias = produtos.Where(x => x.Desistencia == true && x.DataDesistencia >= DateTime.Now.AddDays(-7));
        var totalProdutosDisponiveis7Dias = produtos.Where(x => x.Desistencia == false && x.Ativo == true && x.DataPublicacao >= DateTime.Now.AddDays(-7)).Sum(y => y.Quantidade);
        var totalProdutos7Dias = totalProdutosDesistidos7Dias.Count() + totalProdutosDisponiveis7Dias + totalVendas7Dias.Count();

        // novas campanhas 30 dias
        var novasCampanhasDisponiveis30Dias = campanhas.Where(x => x.Ativo == true && x.DataInicio >= DateTime.Now.AddDays(-30));
        var novasCampanhasEncerradas30Dias = campanhas.Where(x => x.Ativo == false && x.DataInicio >= DateTime.Now.AddDays(-30));
        var totalCampanhas30Dias = novasCampanhasDisponiveis30Dias.Count() + novasCampanhasEncerradas30Dias.Count();

        // Produtos por período
        var totalProdutosVendidosPeriodo = produtosVendidos.Where(x => x.DataVenda >= DateTime.Now.AddMonths(-5));

        // 1
        datas.Clear();
        for (var i = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); i.Month == (DateTime.Now.Month); i = i.AddDays(1))
        {
            datas.Add(i);
        }
        List<VendasPeriodoMesViewModel> listaVendas1PeriodoMesViewModel = new List<VendasPeriodoMesViewModel>();
        foreach (var data in datas)
        {
            VendasPeriodoMesViewModel vendasPeriodo1MesViewModel = new VendasPeriodoMesViewModel
            {
                DataVenda = data,
                Quantidade = totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month) == null ? 0 : totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month).Count()
            };

            listaVendas1PeriodoMesViewModel.Add(vendasPeriodo1MesViewModel);
        }

        // 2
        datas.Clear();
        DateTime dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
        for (var i = dataInicial; i.Month == dataInicial.Month; i = i.AddDays(1))
        {
            datas.Add(i);
        }
        List<VendasPeriodoMesViewModel> listaVendas2PeriodoMesViewModel = new List<VendasPeriodoMesViewModel>();
        foreach (var data in datas)
        {
            VendasPeriodoMesViewModel vendasPeriodo2MesViewModel = new VendasPeriodoMesViewModel
            {
                DataVenda = data,
                Quantidade = totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month) == null ? 0 : totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month).Count()
            };

            listaVendas2PeriodoMesViewModel.Add(vendasPeriodo2MesViewModel);
        }

        // 3
        datas.Clear();
        dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2);
        for (var i = dataInicial; i.Month == dataInicial.Month; i = i.AddDays(1))
        {
            datas.Add(i);
        }
        List<VendasPeriodoMesViewModel> listaVendas3PeriodoMesViewModel = new List<VendasPeriodoMesViewModel>();
        foreach (var data in datas)
        {
            VendasPeriodoMesViewModel vendasPeriodo3MesViewModel = new VendasPeriodoMesViewModel
            {
                DataVenda = data,
                Quantidade = totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month) == null ? 0 : totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month).Count()
            };

            listaVendas3PeriodoMesViewModel.Add(vendasPeriodo3MesViewModel);
        }

        // 4
        datas.Clear();
        dataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-3);
        for (var i = dataInicial; i.Month == dataInicial.Month; i = i.AddDays(1))
        {
            datas.Add(i);
        }
        List<VendasPeriodoMesViewModel> listaVendas4PeriodoMesViewModel = new List<VendasPeriodoMesViewModel>();
        foreach (var data in datas)
        {
            VendasPeriodoMesViewModel vendasPeriodo4MesViewModel = new VendasPeriodoMesViewModel
            {
                DataVenda = data,
                Quantidade = totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month) == null ? 0 : totalProdutosVendidosPeriodo.Where(x => x.DataVenda.Value.Day == data.Day && x.DataVenda.Value.Month == data.Month).Count()
            };

            listaVendas4PeriodoMesViewModel.Add(vendasPeriodo4MesViewModel);
        }


        // Adicionando na ViewModel
        DashboardViewModel dashboardViewModel = new DashboardViewModel();
        dashboardViewModel.NovosCondominos = produtosDesistidos.Count();
        dashboardViewModel.ProdutosVendidos = produtosVendidos.Count();
        dashboardViewModel.ProdutosDisponiveis = produtosDisponiveis;

        dashboardViewModel.NovosCondominos7Dias = listanovosCondominos7DiasViewModel;
        dashboardViewModel.Vendas7DiasViewModel = listaVendas7DiasViewModel;

        dashboardViewModel.TotalProdutosDesistidosUltimos7Dias = totalProdutosDesistidos7Dias.Any() == false ? 0 : ((decimal)totalProdutosDesistidos7Dias.Count() / totalProdutos7Dias) * 10000000;
        dashboardViewModel.TotalProdutosDisponiveisUltimos7Dias = totalProdutosDisponiveis7Dias == 0 ? 0 : ((decimal)totalProdutosDisponiveis7Dias / totalProdutos7Dias) * 10000000;
        dashboardViewModel.TotalProdutosVendidosUltimos7Dias = totalProdutosVendidos7Dias.Any() == false ? 0 : ((decimal)totalProdutosVendidos7Dias.Count() / totalProdutos7Dias) * 10000000;

        dashboardViewModel.NovasCampanhasDisponiveisUlitmos30Dias = novasCampanhasDisponiveis30Dias.Any() == false ? 0 : Math.Round(((decimal)novasCampanhasDisponiveis30Dias.Count() / totalCampanhas30Dias) * 100, 2);

        dashboardViewModel.VendasPeriodo1MesViewModel = listaVendas1PeriodoMesViewModel;
        dashboardViewModel.VendasPeriodo2MesViewModel = listaVendas2PeriodoMesViewModel;
        dashboardViewModel.VendasPeriodo3MesViewModel = listaVendas3PeriodoMesViewModel;
        dashboardViewModel.VendasPeriodo4MesViewModel = listaVendas4PeriodoMesViewModel;

        return View(dashboardViewModel);
    }

}