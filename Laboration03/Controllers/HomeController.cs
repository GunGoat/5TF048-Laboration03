using Laboration03.Infrastructure;
using Laboration03.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Laboration03.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseTest _databaseTest;

        public HomeController(ILogger<HomeController> logger, DatabaseTest databaseTest)
        {
            _logger = logger;
            _databaseTest = databaseTest;
        }

        public IActionResult Index()
        {
            bool canConnect = _databaseTest.CanConnectToDatabase();
            if (canConnect)
            {
                TempData["success"] = "Database connection successful.";
            }
            else
            {
                TempData["error"] = "Database connection failed.";
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
