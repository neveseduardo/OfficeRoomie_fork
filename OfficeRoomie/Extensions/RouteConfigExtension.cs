namespace OfficeRoomie.Extensions;
public static class RouteConfigExtension
{
    public static void RegisterRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
            name: "auth",
            pattern: "{controller=Auth}/{action=Login}/{id?}");

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        endpoints.MapControllerRoute(
            name: "error",
            pattern: "Error/{action=ServerError}/{id?}",
            defaults: new { controller = "Error" });
    }
}