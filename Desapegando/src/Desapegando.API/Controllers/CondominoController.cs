using AutoMapper;
using Desapegando.API.Extensions;
using Desapegando.API.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Desapegando.API.Controllers
{
    [Route("Condomino/[controller]")]
    public class CondominoController : MainController
    {
        private readonly ICondominoRepository _condominoRepository;
        private readonly ICondominoService _condominoService;
        private readonly UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public CondominoController(ICondominoRepository condominoRepository,
                                   IMapper mapper,
                                   IOptions<AppSettings> appSettings,
                                   SignInManager<IdentityUser> signInManager,
                                   UserManager<IdentityUser> userManager,
                                   ICondominoService condominoService,
                                   INotificador notificador) : base(notificador)
        {
            _condominoRepository = condominoRepository;
            _mapper = mapper;
            _userManager = userManager;
            _condominoService = condominoService;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCondominoViewModel>>> Get()
        {
            return Response(_mapper.Map<IEnumerable<GetCondominoViewModel>>(await _condominoRepository.Read()));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetCondominoViewModel>> Get(Guid id)
        {
            return Response(_mapper.Map<GetCondominoViewModel>(await _condominoRepository.ReadById(id)));
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] PostCondominoViewModel condominoViewModel)
        {
            var condominoDb = await _condominoRepository.ReadById(id);

            if (condominoDb == null)
            {
                return Response();
            }

            if (!ModelState.IsValid)
            {
                return Response(ModelState);
            }

            var condomino = _mapper.Map<Condomino>(condominoViewModel);
            condomino.Id = id;

            var validator = new CondominoValidation();
            var resultValidation = validator.Validate(condomino);

            if (!resultValidation.IsValid)
            {
                foreach (var error in resultValidation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return Response(ModelState);
            }

            await _condominoService.Update(condomino);

            if (_notificador.TemNotificacao())
            {
                foreach (var notification in _notificador.GetNotificacoes())
                {
                    ModelState.AddModelError(String.Empty, notification.Mensagem);
                }

                return Response(ModelState);
            }


            if (condominoViewModel.NovaImagem)
            {
                //adicionando claim de foto principal
                var user = await _userManager.FindByEmailAsync(condominoViewModel.Email);

                // Remove a claim antiga
                var existingClaim = await _userManager.GetClaimsAsync(user);
                if (existingClaim.Any(c => c.Type == "ProfilePicture"))
                {
                    var oldClaim = existingClaim.First(c => c.Type == "ProfilePicture");
                    var resultClaim = await _userManager.RemoveClaimAsync(user, oldClaim);
                    if (!resultClaim.Succeeded)
                    {
                        // Lidar com o erro, se necessário
                    }
                }

                // Adiciona a nova claim
                var newClaim = new System.Security.Claims.Claim("ProfilePicture", condomino.ImageFileName);
                var addClaimResult = await _userManager.AddClaimAsync(user, newClaim);
                if (!addClaimResult.Succeeded)
                {
                    // Lidar com o erro ao adicionar a nova claim, se necessário
                }

                await _signInManager.RefreshSignInAsync(user);

                return Response(await CreateJwt(condomino.Email));
            }

            return Response();
        }


        #region MétodosPrivados

        private async Task<UserResponseLogin> CreateJwt(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await GetUserClaims(claims, user);
            var encodedToken = EncodeToken(identityClaims);

            return GetResponseToken(encodedToken, user, claims);
        }

        private UserResponseLogin GetResponseToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UserResponseLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); // quando token irá expirar
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); // quando token foi emitido

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole)); // tratando roles e claims da mesma forma!!!
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            });

            return tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        #endregion
    }
}
