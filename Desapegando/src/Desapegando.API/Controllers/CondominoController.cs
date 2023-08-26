using AutoMapper;
using Desapegando.API.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.API.Controllers
{
    [Route("Condomino/[controller]")]
    public class CondominoController : MainController
    {
        private readonly ICondominoRepository _condominoRepository;
        private readonly ICondominoService _condominoService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CondominoController(ICondominoRepository condominoRepository,
                                   IMapper mapper,
                                   UserManager<IdentityUser> userManager,
                                   ICondominoService condominoService,
                                   INotificador notificador) : base(notificador)
        {
            _condominoRepository = condominoRepository;
            _mapper = mapper;
            _userManager = userManager;
            _condominoService = condominoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCondominoViewModel>>> Get()
        {
            return Response(_mapper.Map<IEnumerable<GetCondominoViewModel>>(await _condominoRepository.Read()));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCondominoViewModel>> Get(Guid id)
        {
            return Response(_mapper.Map<GetCondominoViewModel>(await _condominoRepository.ReadById(id)));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostCondominoViewModel condominoViewModel)
        {
            var condominoDb = await _condominoRepository.ReadById(id);

            if (condominoDb == null)
            {
                return Response();
            }

            if (!ModelState.IsValid)
            {
                return Response(ModelState);
            }

            var condomino = _mapper.Map<Condomino>(condominoViewModel);
            condomino.Id = id;

            var validator = new CondominoValidation();
            var resultValidation = validator.Validate(condomino);

            if (!resultValidation.IsValid)
            {
                foreach (var error in resultValidation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return Response(ModelState);
            }

            await _condominoService.Update(condomino);

            if (_notificador.TemNotificacao())
            {
                foreach (var notification in _notificador.GetNotificacoes())
                {
                    ModelState.AddModelError(String.Empty, notification.Mensagem);
                }

                return Response(ModelState);
            }

            return Response();
        }
    }
}
