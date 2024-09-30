using Microsoft.AspNetCore.Mvc;

namespace Laboration03.Controllers;

public class MovieController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }
}
