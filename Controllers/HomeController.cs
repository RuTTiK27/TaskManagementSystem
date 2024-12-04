using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using TaskManagementSystem.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var userDetails = HttpContext.Session.GetString("UserDetails");
            User user = new Models.User();
            if (userDetails != null)
            {
                user = JsonSerializer.Deserialize<User>(userDetails);
                ViewBag.userDetails = user;
            }
            
            if (isFromLogout!=null)
            {
                if (HttpContext.Session.GetString("UserDetails") != null)
                {
                    HttpContext.Session.Remove("UserDetails");
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExtendSession()
        {
            try
            {
                HttpContext.Session.SetString("UserDetails", HttpContext.Session.GetString("UserDetails")??"");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
