using Microsoft.OpenApi.Models;

namespace OfficeRoomie.Extensions;
public static class SwaggerConfigExtension
{
    public static IServiceCollection AddCustomSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "OfficeRoomie",
                Description = "OfficeRoomie - Web APIs exemplo",
                Version = "v1"
            });
        });

        return services;
    }
}