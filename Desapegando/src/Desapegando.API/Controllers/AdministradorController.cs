using Desapegando.API.Services;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.API.Controllers
{
    [Route("Administrador/[controller]")]
    public class AdministradorController : MainController
    {
        private readonly ICondominoRepository _condominoRepository;
        private readonly ICondominoService _condominoService;
        private readonly IEmailSender _emailSender;

        public AdministradorController(ICondominoRepository condominoRepository,
                                       ICondominoService condominoService,
                                       IEmailSender emailSender,
                                       INotificador notificador) : base(notificador)
        {
            _condominoRepository = condominoRepository;
            _condominoService = condominoService;
            _emailSender = emailSender;
        }

        [HttpPost("AtivarCondomino/")]
        public async Task<IActionResult> AtivarCondomino([FromBody] Guid id)
        {
            var condomino = await _condominoRepository.ReadById(id);

            if (condomino == null)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Condômino não encontrado."));
                return Response();
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


            return Response();
        }

        [HttpPost("ExcluirCondomino")]
        public async Task<IActionResult> ExcluirCondomino([FromBody] Guid id)
        {
            var condomino = await _condominoRepository.ReadById(id);

            if (condomino == null)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Condômino não encontrado."));
                return Response();
            }

            // verificar a melhor forma de fazer caso o email ou deletar apontar um erro.
            try
            {
                await _emailSender.SendEmailAsync(condomino.Email, "Condômino não aprovado", "O seu cadastro em Desapegando não foi aprovado.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _notificador.AdicionarNotificacao(new Notificacao("Erro ao disparar e-mail."));
                return Response();
            }

            try
            {
                await _condominoService.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _notificador.AdicionarNotificacao(new Notificacao("Erro ao deletar usuário e-mail."));
                return Response();
            }


            return Response();
        }
    }
}
