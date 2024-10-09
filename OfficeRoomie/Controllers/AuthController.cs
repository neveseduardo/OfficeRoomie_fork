using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OfficeRoomie.DTO;
using OfficeRoomie.Repositories;

namespace OfficeRoomie.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthRepository _authRepository;

    public AuthController(ILogger<AuthController> logger, IAuthRepository authRepository)
    {
        _logger = logger;
        _authRepository = authRepository;
    }

    [HttpGet("login")]
    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromForm] LoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Dados inválidos fornecidos.");
                return View("Login");
            }

            if (dto == null || string.IsNullOrEmpty(dto.email) || string.IsNullOrEmpty(dto.password))
            {
                ModelState.AddModelError(string.Empty, "O email e a senha são obrigatórios.");
                return View("Login", dto);
            }

            var user = await _authRepository.ValidateAdministradorAsync(dto.email, dto.password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos.");
                return View("Login", dto);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.nome)
            };

            var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var identity = new ClaimsIdentity(claims, authScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(authScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true
            });

            var returnUrl = Request.Form["ReturnUrl"].ToString();
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro durante a autenticação.");
            ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Tente novamente mais tarde.");
            return View("Login");
        }
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/auth/login");
    }
}