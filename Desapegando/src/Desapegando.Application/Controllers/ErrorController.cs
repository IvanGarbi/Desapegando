using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Desapegando.Application.Controllers
{
    public class ErrorController : MainController
    {
        public ErrorController(INotificador notificador) : base(notificador)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
