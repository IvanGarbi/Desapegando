using AutoMapper;
using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers
{
    public class LavanderiaController : MainController
    {
        private readonly IMapper _mapper;

        public LavanderiaController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
