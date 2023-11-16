using AutoMapper;
using Desapegando.API.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Services;
using Desapegando.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.API.Controllers
{
    [Route("Campanha/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCampanhaViewModel>>> Get()
        {
            return Response(_mapper.Map<IEnumerable<GetCampanhaViewModel>>(await _campanhaRepository.Read()));
        }

        [HttpGet("MinhasCampanhas/{condominoId:guid}")]
        public async Task<ActionResult<IEnumerable<GetCampanhaViewModel>>> GetMinhasCampanhas(Guid condominoId)
        {
            return Response(_mapper.Map<IEnumerable<GetCampanhaViewModel>>(await _campanhaRepository.ReadExpression(x => x.CondominoId == condominoId && x.Ativo == true)));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCampanhaViewModel>> Get(Guid id)
        {
            return Response(_mapper.Map<GetCampanhaViewModel>(await _campanhaRepository.ReadById(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostCampanhaViewModel postCampanhaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Response(postCampanhaViewModel);
            }

            if (postCampanhaViewModel.ImagensUploadNames.Any() && postCampanhaViewModel.ImagensUploadNames.Count > 4)
            {
                ModelState.AddModelError("ImagensUploadNames", "Só é possível adicionar no máximo 4 imagens.");
                return Response(postCampanhaViewModel);
            }

            var campanha = _mapper.Map<Campanha>(postCampanhaViewModel);

            await _campanhaService.Create(campanha);



            if (!_notificador.TemNotificacao())
            {
                foreach (var imagemNome in postCampanhaViewModel.ImagensUploadNames)
                {
                    var campanhaImagem = new CampanhaImagem();
                    campanhaImagem.FileName = imagemNome;
                    campanhaImagem.CampanhaId = campanha.Id;

                    await _campanhaImagemService.Create(campanhaImagem);
                }

                //return Response();
                return ResponseCeated();
            }

            foreach (var error in _notificador.GetNotificacoes())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return Response(postCampanhaViewModel);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] PatchCampanhaViewModel campanhaViewModel)
        {
            bool novasImagens = campanhaViewModel.ImagensUploadNames.Any();

            if (!novasImagens)
            {
                ModelState.ClearValidationState("ImagensUpload");
                ModelState.MarkFieldValid("ImagensUpload");
            }

            if (!ModelState.IsValid)
            {
                return Response(campanhaViewModel);
            }

            if (novasImagens && campanhaViewModel.ImagensUploadNames.Count > 4)
            {
                ModelState.AddModelError("ImagensUploadNames", "Só é possível adicionar no máximo 4 imagens.");
                return Response(campanhaViewModel);
            }

            var campanhaDb = await _campanhaRepository.ReadById(campanhaViewModel.Id);

            if (campanhaDb == null)
            {
                ModelState.AddModelError(string.Empty, "Campanha não encontrado.");
                return Response(campanhaViewModel);
            }

            MapearCampanha(campanhaDb, campanhaViewModel);

            await _campanhaService.Update(campanhaDb);

            if (!_notificador.TemNotificacao())
            {
                if (novasImagens)
                {
                    var listaProdutoImagensDb = campanhaDb.CampanhaImagens;
                    // Deletando imagens antigas
                    foreach (var imagem in listaProdutoImagensDb)
                    {
                        await _campanhaImagemService.Delete(imagem.Id);
                    }

                    // Adicionando as imagens novas
                    foreach (var imagemNome in campanhaViewModel.ImagensUploadNames)
                    {
                        var campanhaImagem = new CampanhaImagem();
                        campanhaImagem.FileName = imagemNome;
                        campanhaImagem.CampanhaId = campanhaDb.Id;

                        await _campanhaImagemService.Create(campanhaImagem);
                    }
                }

                return Response();
            }

            foreach (var error in _notificador.GetNotificacoes())
            {
                ModelState.AddModelError(error.Propriedade, error.Mensagem);
            }

            return Response(campanhaViewModel);

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _campanhaService.Delete(id);

            if (_notificador.TemNotificacao())
            {
                foreach (var error in _notificador.GetNotificacoes())
                {
                    ModelState.AddModelError(error.Propriedade, error.Mensagem);
                };

                return Response(ModelState);
            }

            return Response();
        }

        #region Métodos Privados
        private static void MapearCampanha(Campanha campanha, PatchCampanhaViewModel campanhaViewModel)
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
