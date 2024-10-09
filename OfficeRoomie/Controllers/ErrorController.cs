using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    public new IActionResult NotFound()
    {
        return View();
    }

    public new IActionResult Unauthorized()
    {
        return View();
    }

    public IActionResult Forbidden()
    {
        return View();
    }

    public IActionResult Server()
    {
        return View();
    }
}