using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string isFromLogout)
        {
            if (isFromLogout!=null)
            {
                if (HttpContext.Session.GetString("Email") != null)
                {
                    HttpContext.Session.Remove("Email");
                }

                if (HttpContext.Session.GetString("userProfile") != null)
                {
                    HttpContext.Session.Remove("userProfile");
                }
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
