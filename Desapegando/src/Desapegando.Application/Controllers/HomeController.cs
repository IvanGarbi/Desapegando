using Desapegando.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Desapegando.Business.Interfaces.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Desapegando.Data.Repository;
using Desapegando.Business.Interfaces.Services;

namespace Desapegando.Application.Controllers
{
    public class HomeController : MainController
    {
        private readonly ICondominoRepository _condominoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ICondominoRepository condominoRepository, IMapper mapper, UserManager<IdentityUser> userManager, IProdutoRepository produtoRepository)
        {
            _condominoRepository = condominoRepository;
            _mapper = mapper;
            _userManager = userManager;
            _produtoRepository = produtoRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<GetProdutoViewModel>>(await _produtoRepository.Read()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}