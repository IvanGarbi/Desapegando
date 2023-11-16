using AutoMapper;
using Desapegando.API.Extensions;
using Desapegando.API.ViewModels;
using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Notifications;
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
    [Route("Auth/[controller]")]
    public class AuthController : MainController
    {
        private readonly ICondominoService _condominoService;
        private readonly ICondominoRepository _condominoRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings,
                              ICondominoService condominoService,
                              ICondominoRepository condominoRepository,
                              IMapper mapper,
                              INotificador notificador) : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _condominoService = condominoService;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _condominoRepository = condominoRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CondominoRegisterViewModel condominoRegisterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = condominoRegisterViewModel.Email, // aqui podemos verificar pra colocar o nome do usuário.
                Email = condominoRegisterViewModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, condominoRegisterViewModel.Senha);

            if (result.Succeeded)
            {
                var identity = await _userManager.FindByEmailAsync(condominoRegisterViewModel.Email);
                var condomino = _mapper.Map<Condomino>(condominoRegisterViewModel);

                condomino.Id = Guid.Parse(identity.Id);

                condomino.Telefone = condomino.Telefone.Replace("-", "");
                condomino.Telefone = condomino.Telefone.Replace("(", "");
                condomino.Telefone = condomino.Telefone.Replace(")", "");
                condomino.Telefone = condomino.Telefone.Replace(" ", "");
                condomino.Cpf = condomino.Cpf.Replace(".", "");
                condomino.Cpf = condomino.Cpf.Replace("-", "");

                var validator = new CondominoValidation();
                var resultValidation = validator.Validate(condomino);

                if (resultValidation.IsValid)
                {
                    try
                    {
                        // adicionando claim de foto principal
                        await _userManager.AddClaimAsync(identity, new Claim("ProfilePicture", condomino.ImageFileName));

                        await _condominoService.Create(condomino);

                        if (!_notificador.TemNotificacao())
                        {
                            // aqui retornamos o JWT para após o Registro já ser feito o Login na aplicação!
                            //return Response(await CreateJwt(condomino.Email));
                            return ResponseCeated(await CreateJwt(condomino.Email)); //verificar
                        }

                        await _userManager.DeleteAsync(user);

                        foreach (var error in resultValidation.Errors)
                        {
                            _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
                        }

                        return Response();

                    }
                    catch (Exception ex)
                    {
                        await _userManager.DeleteAsync(user);

                        // mostrar erro ao usuário
                        _notificador.AdicionarNotificacao(new Notificacao("Erro ao registrar usuário."));

                        return Response();
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(user);

                    foreach (var error in resultValidation.Errors)
                    {
                        _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
                    }

                    return Response();
                }
            }
            else
            {
                // mostrar erro ao usuário
                foreach (var error in result.Errors)
                {
                    _notificador.AdicionarNotificacao(new Notificacao(error.Description));
                }

                return Response();
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(CondominoLoginViewModel condominoLoginViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var condomino = await _condominoRepository.ReadWithExpression(x => x.Email == condominoLoginViewModel.Email);

            if (condomino != null)
            {
                if (condomino.Ativo)
                {
                    var result = await _signInManager.PasswordSignInAsync(condominoLoginViewModel.Email, condominoLoginViewModel.Senha, false, true);

                    if (result.Succeeded)
                    {
                        return Response(await CreateJwt(condominoLoginViewModel.Email));
                    }

                    if (result.IsLockedOut)
                    {
                        _notificador.AdicionarNotificacao(new Notificacao("Usuário temporariamente bloqueado por tentativas inválidas"));
                        return Response();
                    }
                }
                else
                {
                    _notificador.AdicionarNotificacao(new Notificacao("Usuário não ativado pelo síndico."));
                    return Response();
                }
            }

            _notificador.AdicionarNotificacao(new Notificacao("Usuário ou Senha incorretos"));
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


public class UserResponseLogin
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken UserToken { get; set; }
}

public class UserToken
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserClaim> Claims { get; set; }
}

public class UserClaim
{
    public string Value { get; set; }
    public string Type { get; set; }
}
