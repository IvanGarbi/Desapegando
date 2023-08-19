using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected readonly INotificador _notificador;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected ActionResult Response(object result = null)
        {
            if (_notificador.TemNotificacao())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = new Dictionary<string, string[]>
            {
                { "Messages", _notificador.GetNotifications().Select(n => n.Mensagem).ToArray() }
            }
            });
        }
    }
}
