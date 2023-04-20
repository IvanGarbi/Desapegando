using Desapegando.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Desapegando.Business.Interfaces.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Desapegando.Data.Repository;

namespace Desapegando.Application.Controllers
{
    public class HomeController : MainController
    {
        private readonly ICondominoRepository _condominoRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ICondominoRepository condominoRepository, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _condominoRepository = condominoRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<CondominoViewModel>(await _condominoRepository.ReadById(Guid.Parse(_userManager.GetUserId(User)))));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}