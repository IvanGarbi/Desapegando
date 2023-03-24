﻿using AutoMapper;
using Desapegando.Application.Models;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Desapegando.Application.Controllers;

public class LoginController : MainController
{
    private readonly ICondominoService _condominoService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IMapper _mapper;

    public LoginController(UserManager<IdentityUser> userManager, ICondominoService condominoService, SignInManager<IdentityUser> signInManager, IMapper mapper)
    {
        _userManager = userManager;
        _condominoService = condominoService;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CondominoViewModel condominoViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(condominoViewModel);
        }

        var user = new IdentityUser();
        user.Email = condominoViewModel.Email;
        user.PhoneNumber = condominoViewModel.Telefone;
        user.UserName = condominoViewModel.Nome;

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            var identity = await _userManager.FindByEmailAsync(condominoViewModel.Email);
            var condomino = _mapper.Map<Condomino>(condominoViewModel);

            condomino.Id = Guid.Parse(identity.Id);

            await _condominoService.Create(condomino);
        }

        return View();
    }

}