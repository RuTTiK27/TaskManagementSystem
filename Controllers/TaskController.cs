using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult UserDashboard()
        {
            return View();
        }
        public IActionResult AddTask()
        {
            return View();
        }
    }
}
