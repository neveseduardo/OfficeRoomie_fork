namespace OfficeRoomie.Middleware;

public class ErrorStatusCodeMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorStatusCodeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.HasStarted)
        {
            return;
        }

        switch (context.Response.StatusCode)
        {
            case StatusCodes.Status404NotFound:
                context.Response.Redirect("/Error/NotFound");
                break;
            case StatusCodes.Status401Unauthorized:
                context.Response.Redirect("/Auth/Login");
                break;
            case StatusCodes.Status403Forbidden:
                context.Response.Redirect("/Error/Forbidden");
                break;
            case StatusCodes.Status500InternalServerError:
                context.Response.Redirect("/Error/ServerError");
                break;
        }
    }
}