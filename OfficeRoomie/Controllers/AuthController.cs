using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace OfficeRoomie.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpGet("login")]
    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromForm] IFormCollection formBody)
    {
        if (formBody["email"] != "email@email.com" || formBody["password"] != "admin")
        {
            ViewBag.Fail = true;

            return View("Login");
        }

        var user = new
        {
            Id = Guid.NewGuid(),
            Name = "Administrador"
        };

        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        ];

        var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        var identity = new ClaimsIdentity(claims, authScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(authScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true
        });

        var returnUrl = formBody["ReturnUrl"].ToString();

        if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }


        return Redirect("/");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return Redirect("/auth/login");
    }
}
