﻿using AutoMapper;
using Desapegando.Application.Extensions;
using Desapegando.Application.Services.MVC;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Controllers;

public class CampanhaController : MainController
{
    private readonly IMapper _mapper;
    private readonly ICampanhaService _campanhaService;

    public CampanhaController(ICampanhaService campanhaService,
                              IMapper mapper)
    {
        _mapper = mapper;
        _campanhaService = campanhaService;
    }

    public async Task<IActionResult> Criar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CampanhaViewModel campanhaViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(campanhaViewModel);
        }

        if (campanhaViewModel.ImagensUpload.Any() && campanhaViewModel.ImagensUpload.Count > 4)
        {
            ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
            return View(campanhaViewModel);
        }

        var postCampanhaViewModel = _mapper.Map<PostCampanhaViewModel>(campanhaViewModel);

        postCampanhaViewModel.CondominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        postCampanhaViewModel.ImagensUploadNames = new List<string>();

        foreach (var imagem in campanhaViewModel.ImagensUpload)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(imagem, imgPrefixo))
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                return View(campanhaViewModel);
            }

            postCampanhaViewModel.ImagensUploadNames.Add(imgPrefixo + imagem.FileName);
        }

        var response = await _campanhaService.CriarCampanha(postCampanhaViewModel);

        if (ResponsePossuiErros(response))
        {
            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(campanhaViewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> MinhasCampanhas()
    {
        var condominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        var response = await _campanhaService.GetMinhasCampanhas(condominoId);

        return View(response.Data);
    }

    public async Task<IActionResult> Editar(Guid id)
    {
        var response = await _campanhaService.GetCampanha(id);

        if (!response.Success)
        {
            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View();
        }

        var campanha = _mapper.Map<Campanha>(response.Data);

        var updateCampanhaViewModel = _mapper.Map<UpdateCampanhaViewModel>(campanha);

        return View(updateCampanhaViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(UpdateCampanhaViewModel campanhaViewModel)
    {
        bool novasImagens = campanhaViewModel.ImagensUpload != null;

        if (!novasImagens)
        {
            ModelState.ClearValidationState("ImagensUpload");
            ModelState.MarkFieldValid("ImagensUpload");
        }

        if (!ModelState.IsValid)
        {
            return View(campanhaViewModel);
        }

        if (novasImagens && campanhaViewModel.ImagensUpload.Count > 4)
        {
            ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
            return View(campanhaViewModel);
        }

        var patchCampanhaViewModel = _mapper.Map<PatchCampanhaViewModel>(campanhaViewModel);

        patchCampanhaViewModel.CondominoId = Guid.Parse(User.FindFirst("sub")?.Value);

        patchCampanhaViewModel.ImagensUploadNames = new List<string>();

        if (novasImagens)
        {
            foreach (var imagem in campanhaViewModel.ImagensUpload)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(imagem, imgPrefixo))
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                    return View(campanhaViewModel);
                }

                patchCampanhaViewModel.ImagensUploadNames.Add(imgPrefixo + imagem.FileName);
            }

            var responseCampanha = await _campanhaService.GetCampanha(campanhaViewModel.Id);

            GetCampanhaResponseId campanhaDb;

            campanhaDb = responseCampanha;


            if (!campanhaDb.Success)
            {
                return View(campanhaViewModel);
            }

            var listaProdutoImagensDb = campanhaDb.Data.CampanhaImagemViewModels;
            // Deletando imagens antigas
            foreach (var imagem in listaProdutoImagensDb)
            {
                bool result = await DeletarArquivo(imagem.FileName);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");
                }
            }
        }

        var response = await _campanhaService.UpdateCampanha(patchCampanhaViewModel);

        if (ResponsePossuiErros(response))
        {
            ViewBag.Error = "Ocorreu um erro ao salvar";

            return View(campanhaViewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Visualizar(Guid id)
    {
        var response = await _campanhaService.GetCampanha(id);

        return View(response.Data);
    }

    public async Task<IActionResult> Deletar(Guid id)
    {
        var response = await _campanhaService.DeletarCampanha(id);

        if (ResponsePossuiErros(response))
        {
            return View();
        }

        return RedirectToAction("MinhasCampanhas");
    }

    public async Task<IActionResult> Campanhas()
    {
        var response = await _campanhaService.GetCampanhas();

        var campanhasDb = response.Data.Where(x => x.Ativo);

        if (!campanhasDb.Any())
        {
            ViewBag.Campanhas = Enumerable.Empty<GetCampanhaViewModel>();
            
            return View();
        }

        ViewBag.Campanhas = campanhasDb;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Campanhas(FiltrarCampanhaViewModel filtrarCampanhaViewModel)
    {
        if (string.IsNullOrEmpty(filtrarCampanhaViewModel.Nome) && string.IsNullOrEmpty(filtrarCampanhaViewModel.NomeInstituicao))
        {
            return RedirectToAction("Campanhas");
        }

        var response = await _campanhaService.GetCampanhas();

        var campanhasDb = response.Data.Where(x => ((!string.IsNullOrEmpty(filtrarCampanhaViewModel.Nome) && x.Nome.ToLower().Trim().Contains(filtrarCampanhaViewModel.Nome.ToLower().Trim())) ||
                                                   (!string.IsNullOrEmpty(filtrarCampanhaViewModel.NomeInstituicao) && x.NomeInstituicao.ToLower().Trim().Contains(filtrarCampanhaViewModel.NomeInstituicao.ToLower().Trim())))
                                                   && x.Ativo == true);

        if (!campanhasDb.Any())
        {
            ViewBag.Campanhas = Enumerable.Empty<GetCampanhaViewModel>();
            
            return View();
        }

        var campanhasViewModel = _mapper.Map<IEnumerable<GetCampanhaViewModel>>(campanhasDb);

        ViewBag.Campanhas = campanhasViewModel;

        return View();
    }

    #region MétodosPrivados
    private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagens", imgPrefixo + arquivo.FileName);

        //verificar se o arquivo já existe no diretório
        if (System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
            return false;
        }


        // gravando em "disco"
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }

    private async Task<bool> DeletarArquivo(string imagem)
    {
        if (imagem.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imagem);

        //verificar se o arquivo já existe no diretório
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            return true;
        }

        return false;
    }

    private static void MapearCampanha(Campanha campanha, UpdateCampanhaViewModel campanhaViewModel)
    {
        campanha.Nome = campanhaViewModel.Nome;
        campanha.Descricao = campanhaViewModel.Descricao;
        campanha.Ativo = campanhaViewModel.Ativo;
        campanha.NomeInstituicao = campanhaViewModel.NomeInstituicao;
        campanha.DataInicio = campanhaViewModel.DataInicio.Value;
        campanha.DataFinal = campanhaViewModel.DataFinal.Value;
        campanha.EmailResponsavel = campanhaViewModel.EmailResponsavel;
        campanha.LocalDeEncontro = campanhaViewModel.LocalDeEncontro;
        campanha.NomeResponsavel = campanhaViewModel.NomeResponsavel;
        campanha.TelefoneResponsavel = campanhaViewModel.TelefoneResponsavel;
    }
    #endregion
}

