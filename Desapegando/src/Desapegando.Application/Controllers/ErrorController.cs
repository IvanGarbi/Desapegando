using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System.Net.Http;

namespace Desapegando.Application.Controllers
{
    public class ErrorController : MainController
    {
        public ErrorController()
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
