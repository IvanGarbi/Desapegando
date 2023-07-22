using AutoMapper;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Desapegando.Application.Controllers
{
    public class CampanhaController : MainController
    {
        private readonly ICampanhaRepository _campanhaRepository;
        private readonly ICampanhaService _campanhaService;
        private readonly ICampanhaImagemService _campanhaImagemService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CampanhaController(ICampanhaRepository campanhaRepository, 
                                  ICampanhaService campanhaService,
                                  IMapper mapper, 
                                  UserManager<IdentityUser> userManager, 
                                  ICampanhaImagemService campanhaImagemService,
                                  INotificador notificador) : base(notificador)
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

            if (campanhaViewModel.ImagensUpload.Any() && campanhaViewModel.ImagensUpload.Count > 4)
            {
                ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
                return View(campanhaViewModel);
            }

            var campanha = _mapper.Map<Campanha>(campanhaViewModel);

            campanha.CondominoId = Guid.Parse(_userManager.GetUserId(this.User));

            await _campanhaService.Create(campanha);

            if (!_notificador.TemNotificacao())
            {
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

            foreach (var error in _notificador.GetNotifications())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return View(campanhaViewModel);
        }

        public async Task<IActionResult> MinhasCampanhas()
        {
            var todosCampanhaDb = await _campanhaRepository.ReadExpression(x => x.CondominoId == Guid.Parse(_userManager.GetUserId(this.User)) && x.Ativo == true);

            var meusCampanhasDbViewModel = _mapper.Map<IEnumerable<GetCampanhaViewModel>>(todosCampanhaDb);

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

            if (novasImagens && campanhaViewModel.ImagensUpload.Count > 4)
            {
                ModelState.AddModelError("ImagensUpload", "Só é possível adicionar no máximo 4 imagens.");
                return View(campanhaViewModel);
            }

            var campanhaDb = await _campanhaRepository.ReadById(campanhaViewModel.Id);

            if (campanhaDb == null)
            {
                return View(campanhaViewModel);
            }

            MapearCampanha(campanhaDb, campanhaViewModel);

            await _campanhaService.Update(campanhaDb);

            if (!_notificador.TemNotificacao())
            {
                if (novasImagens)
                {
                    var listaCampanhaImagensDb = campanhaDb.CampanhaImagens;
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

            foreach (var error in _notificador.GetNotifications())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return View(campanhaViewModel);
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

        public async Task<IActionResult> Deletar(Guid id)
        {
            await _campanhaService.Delete(id);

            if (!_notificador.TemNotificacao())
            {
                return RedirectToAction("MinhasCampanhas");
            }

            return View();
        }

        public async Task<IActionResult> Campanhas()
        {
            var campanhasDb = await _campanhaRepository.ReadExpression(x => x.Ativo);

            ViewBag.Campanhas = Enumerable.Empty<GetCampanhaViewModel>();

            if (!campanhasDb.Any())
            {
                return View();
            }

            var campanhasViewModel = _mapper.Map<IEnumerable<GetCampanhaViewModel>>(campanhasDb);

            ViewBag.Campanhas = campanhasViewModel;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Campanhas(FiltrarCampanhaViewModel filtrarCampanhaViewModel)
        {
            if (filtrarCampanhaViewModel.Nome == null || string.IsNullOrEmpty(filtrarCampanhaViewModel.Nome))
            {
                return RedirectToAction("Campanhas");
            }

            var campanhasDb = await _campanhaRepository.ReadExpression(x => x.Nome.ToLower().Trim().Contains(filtrarCampanhaViewModel.Nome.ToLower().Trim()) && x.Ativo == true);

            ViewBag.Campanhas = Enumerable.Empty<GetCampanhaViewModel>();

            if (!campanhasDb.Any())
            {
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
}
