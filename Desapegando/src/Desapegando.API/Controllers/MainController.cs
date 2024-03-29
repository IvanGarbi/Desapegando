﻿using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Desapegando.API.Controllers
{
    [Authorize]
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
            if (!_notificador.TemNotificacao())
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
                { "Messages", _notificador.GetNotificacoes().Select(n => n.Mensagem).ToArray() }
            }
            });
        }

        protected ActionResult ResponseCeated(object result = null)
        {
            return Created("", new
            {
                success = true,
                data = result
            });
        }
    }
}
