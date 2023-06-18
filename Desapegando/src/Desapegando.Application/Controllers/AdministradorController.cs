﻿using AutoMapper;
using Desapegando.Application.ViewModels;
using Desapegando.Application.Services;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class AdministradorController : MainController
{
    private readonly ICondominoRepository _condominoRepository;
    private readonly ICampanhaRepository _campanhaRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICondominoService _condominoService;
    private readonly IMapper _mapper;

    private readonly IEmailSender _emailSender;
    public AdministradorController(ICondominoRepository condominoRepository, 
                                   IMapper mapper, 
                                   ICondominoService condominoService, 
                                   IEmailSender emailSender, 
                                   ICampanhaRepository campanhaRepository, 
                                   IProdutoRepository produtoRepository)
    {
        _condominoRepository = condominoRepository;
        _mapper = mapper;
        _condominoService = condominoService;
        _emailSender = emailSender;
        _campanhaRepository = campanhaRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<IActionResult> NovosCondominos()
    {
        var condominosInativos = await _condominoRepository.ReadWithExpressionList(x => x.Ativo == false);

        //var condominosInativos = condominos.Where(x => x.Ativo == false);

        return View(_mapper.Map<IEnumerable<CondominoInativoViewModel>>(condominosInativos));
    }

    //[HttpPost]
    public async Task<IActionResult> AtivarCondomino(Guid id)
    {
        var condomino = await _condominoRepository.ReadById(id);

        if (condomino == null)
        {
            return RedirectToAction("NovosCondominos", "Administrador");
        }

        condomino.Ativo = true;

        await _condominoService.Update(condomino);

        try
        {
            await _emailSender.SendEmailAsync(condomino.Email, "Condômino aprovado", "O seu cadastro em Desapegando já foi aprovado! Já é possível realizar o login e desfrutar da plataforma!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        return RedirectToAction("NovosCondominos", "Administrador");
    }

    public async Task<IActionResult> ExcluirCondomino(Guid id)
    {
        var condomino = await _condominoRepository.ReadById(id);

        if (condomino == null)
        {
            return RedirectToAction("NovosCondominos", "Administrador");
        }

        await _condominoService.Delete(id);

        try
        {
            await _emailSender.SendEmailAsync(condomino.Email, "Condômino não aprovado", "O seu cadastro em Desapegando não foi aprovado.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        return RedirectToAction("NovosCondominos", "Administrador");
    }

    public async Task<IActionResult> Dashboard()
    {
        var produtos = await _produtoRepository.Read();
        var campanhas = await _campanhaRepository.Read();
        var condominos = await _condominoRepository.Read();

        // primeiros resultados
        var produtosDesistidos = produtos.Where(x => x.Desistencia == true);
        var produtosVendidos = produtos.Where(x => x.Desistencia == false && x.Ativo == false);
        var produtosDisponiveis = produtos.Where(x => x.Ativo == true);

        // Novos condôminos 7 dias
        var novosCondominos7Dias = condominos.Where(x => x.Ativo == false && x.DataRegistro >= DateTime.Now.AddMonths(-2)).ToArray(); // pegar de 2 meses para facilitar, pois a iteração da data é quem define

        var datas = new List<DateTime>();
        //datas = novosCondominos7Dias.Select(x => x.DataRegistro).Distinct().ToList();
        for (int i = 7; i > 0; i--)
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
                Quantidade = novosCondominos7Dias.Where(x => x.DataRegistro.Day == data.Day) == null ? 0 : novosCondominos7Dias.Where(x => x.DataRegistro.Day == data.Day && x.DataRegistro.Month == data.Month).Count()
            };

            listanovosCondominos7DiasViewModel.Add(novosCondominos7DiasViewModel);
        }

        listanovosCondominos7DiasViewModel = listanovosCondominos7DiasViewModel.OrderBy(x => x.DataRegistro).ToList();

        // total vendas 7 dias
        var totalVendas7Dias = produtos.Where(x => x.Desistencia == false && x.Ativo == false && x.DataVenda >= DateTime.Now.AddDays(-8));
        datas.Clear();
        for (int i = 7; i > 0; i--)
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
        var totalProdutosDesistidos7Dias = produtos.Where(x => x.Desistencia == true && x.DataVenda >= DateTime.Now.AddDays(-7));
        var totalProdutosDisponiveis7Dias = produtos.Where(x => x.Desistencia == false && x.Ativo == true && x.DataPublicacao >= DateTime.Now.AddDays(-7));
        var totalProdutos7Dias = totalProdutosDesistidos7Dias.Count() + totalProdutosDisponiveis7Dias.Count() + totalVendas7Dias.Count();

        // novas campanhas 30 dias
        var novasCampanhasDisponiveis30Dias = campanhas.Where(x => x.Ativo == true && x.DataInicio >= DateTime.Now.AddDays(-30));
        var novasCampanhasEncerradas30Dias = campanhas.Where(x => x.Ativo == false && x.DataInicio >= DateTime.Now.AddDays(-30));
        var totalCampanhas30Dias = novasCampanhasDisponiveis30Dias.Count() + novasCampanhasEncerradas30Dias.Count();

        DashboardViewModel dashboardViewModel = new DashboardViewModel();
        dashboardViewModel.NovosCondominos = produtosDesistidos.Count();
        dashboardViewModel.ProdutosVendidos = produtosVendidos.Count();
        dashboardViewModel.ProdutosDisponiveis = produtosDisponiveis.Count();

        dashboardViewModel.NovosCondominos7Dias = listanovosCondominos7DiasViewModel;
        dashboardViewModel.Vendas7DiasViewModel = listaVendas7DiasViewModel;
        
        dashboardViewModel.TotalProdutosDesistidosUltimos7Dias = totalProdutosDesistidos7Dias.Any() == false ? 0 : ((decimal)totalProdutosDesistidos7Dias.Count() / totalProdutos7Dias) * 10000000;
        dashboardViewModel.TotalProdutosDisponiveisUltimos7Dias = totalProdutosDisponiveis7Dias.Any() == false ? 0 : ((decimal)totalProdutosDisponiveis7Dias.Count() / totalProdutos7Dias) * 10000000;
        dashboardViewModel.TotalProdutosVendidosUltimos7Dias = totalProdutosVendidos7Dias.Any() == false ? 0 : ((decimal)totalProdutosVendidos7Dias.Count() / totalProdutos7Dias) * 10000000;

        dashboardViewModel.NovasCampanhasDisponiveisUlitmos30Dias = novasCampanhasDisponiveis30Dias.Any() == false ? 0 : (novasCampanhasDisponiveis30Dias.Count() / totalCampanhas30Dias) * 100;

        return View(dashboardViewModel);
    }

}