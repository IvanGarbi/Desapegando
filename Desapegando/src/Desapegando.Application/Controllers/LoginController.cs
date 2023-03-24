using Desapegando.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class LoginController : MainController
{
    public LoginController()
    {
        
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
}