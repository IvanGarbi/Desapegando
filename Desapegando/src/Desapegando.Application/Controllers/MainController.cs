using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

[Authorize]
public abstract class MainController : Controller
{
    protected readonly INotificador _notificador;

    public MainController(INotificador notificador)
    {
        _notificador = notificador;
    }
}