using Microsoft.AspNetCore.Authentication.Cookies;

namespace OfficeRoomie.Extensions;
public static class AuthorizeConfigExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/auth/login";
        });


        return services;
    }
}