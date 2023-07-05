using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public abstract class MainController : Controller
{
    protected readonly INotificador _notificador;

    public MainController(INotificador notificador)
    {
        _notificador = notificador;
    }
}