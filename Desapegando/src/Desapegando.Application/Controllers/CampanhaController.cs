using AutoMapper;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers
{
    public class CampanhaController : MainController
    {
        private readonly ICampanhaRepository _campanhaRepository;
        private readonly ICampanhaService _campanhaService;
        private readonly ICampanhaImagemService _campanhaImagemService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CampanhaController(ICampanhaRepository campanhaRepository, ICampanhaService campanhaService, IMapper mapper, UserManager<IdentityUser> userManager, ICampanhaImagemService campanhaImagemService)
        {
            _campanhaRepository = campanhaRepository;
            _campanhaService = campanhaService;
            _mapper = mapper;
            _userManager = userManager;
            _campanhaImagemService = campanhaImagemService;
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

            if (campanhaViewModel.ImagensUpload.Count > 4)
            {
                ModelState.AddModelError(string.Empty, "Só é possível adicionar no máximo 3 iamgens.");
                return View(campanhaViewModel);
            }

            var campanha = _mapper.Map<Campanha>(campanhaViewModel);

            campanha.CondominoId = Guid.Parse(_userManager.GetUserId(this.User));

            var validator = new CampanhaValidation();
            var resultValidation = validator.Validate(campanha);

            if (!resultValidation.IsValid)
            {
                foreach (var error in resultValidation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(campanhaViewModel);
            }

            await _campanhaService.Create(campanha);

            foreach (var imagem in campanhaViewModel.ImagensUpload)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(imagem, imgPrefixo))
                {
                    await _campanhaService.Delete(campanha.Id);

                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                    return View(campanhaViewModel);
                }

                var campanhaImagem = new CampanhaImagem();
                campanhaImagem.FileName = imgPrefixo + imagem.FileName;
                campanhaImagem.CampanhaId = campanha.Id;

                await _campanhaImagemService.Create(campanhaImagem);

            }

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> MinhasCampanhas()
        {
            var todosCampanhaDb = await _campanhaRepository.Read();

            var minhasCampanhasDb = todosCampanhaDb.Where(x => x.CondominoId == Guid.Parse(_userManager.GetUserId(this.User)));

            if (minhasCampanhasDb == null)
            {
                return View();
            }

            var meusCampanhasDbViewModel = _mapper.Map<IEnumerable<GetCampanhaViewModel>>(minhasCampanhasDb);

            return View(meusCampanhasDbViewModel);
        }


        public async Task<IActionResult> Editar(Guid id)
        {
            var campanhaDb = await _campanhaRepository.ReadById(id);

            if (campanhaDb == null)
            {
                return View();
            }

            var campanhaViewModel = _mapper.Map<UpdateCampanhaViewModel>(campanhaDb);

            return View(campanhaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UpdateCampanhaViewModel campanhaViewModel)
        {
            var novasImagens = campanhaViewModel.ImagensUpload != null;

            if (!novasImagens)
            {
                ModelState.ClearValidationState("ImagensUpload");
                ModelState.MarkFieldValid("ImagensUpload");
            }

            if (!ModelState.IsValid)
            {
                return View(campanhaViewModel);
            }

            if (campanhaViewModel.ImagensUpload.Count > 4)
            {
                ModelState.AddModelError(string.Empty, "Só é possível adicionar no máximo 3 iamgens.");
                return View(campanhaViewModel);
            }


            var campanhaDb = await _campanhaRepository.ReadById(campanhaViewModel.Id);

            var listaCampanhaImagensDb = campanhaDb.CampanhaImagens;

            if (campanhaDb == null)
            {
                return View(campanhaViewModel);
            }

            campanhaDb.Nome = campanhaViewModel.Nome;
            campanhaDb.Descricao = campanhaViewModel.Descricao;
            campanhaDb.Ativo = campanhaViewModel.Ativo;
            campanhaDb.NomeInstituicao = campanhaViewModel.NomeInstituicao;
            campanhaDb.DataInicio = campanhaViewModel.DataInicio;
            campanhaDb.DataFinal = campanhaViewModel.DataFinal;
            campanhaDb.EmailResponsavel = campanhaViewModel.EmailResponsavel;
            campanhaDb.LocalDeEncontro = campanhaViewModel.LocalDeEncontro;
            campanhaDb.NomeResponsavel = campanhaViewModel.NomeResponsavel;
            campanhaDb.TelefoneResponsavel = campanhaViewModel.TelefoneResponsavel;

            var validator = new CampanhaValidation();
            var resultValidation = validator.Validate(campanhaDb);

            if (!resultValidation.IsValid)
            {
                foreach (var error in resultValidation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(campanhaViewModel);
            }

            await _campanhaService.Update(campanhaDb);

            if (novasImagens)
            {
                var imagensAntigasIdLista = new List<Guid>();
                // Deletando imagens antigas
                foreach (var imagem in listaCampanhaImagensDb)
                {
                    bool result = await DeletarArquivo(imagem.FileName);

                    if (!result)
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");
                    }

                    imagensAntigasIdLista.Add(imagem.Id);


                }

                // Adicionando as imagens novas
                foreach (var imagem in campanhaViewModel.ImagensUpload)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(imagem, imgPrefixo))
                    {
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as imagens.");

                        return View(campanhaViewModel);
                    }

                    var campanhaImagem = new CampanhaImagem();
                    campanhaImagem.FileName = imgPrefixo + imagem.FileName;
                    campanhaImagem.CampanhaId = campanhaDb.Id;

                    await _campanhaImagemService.Create(campanhaImagem);
                }

                foreach (var imagemId in imagensAntigasIdLista)
                {
                    await _campanhaImagemService.Delete(imagemId);
                }

            }

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Visualizar(Guid id)
        {
            var campanhaDb = await _campanhaRepository.ReadById(id);

            if (campanhaDb == null)
            {
                return View();
            }

            var campanhaViewModel = _mapper.Map<GetCampanhaViewModel>(campanhaDb);

            return View(campanhaViewModel);
        }

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
    }
}
