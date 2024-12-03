using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;


namespace TaskManagementSystem.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITask _taskRepository; 
        private readonly ILogger<UserController> _logger;
        public TaskController(ITask taskRepository, ILogger<UserController> logger)
        {
                _taskRepository = taskRepository;
                _logger = logger;
                _logger = logger;
        }
        public IActionResult UserDashboard()
        {
            var userDetails = HttpContext.Session.GetString("UserDetails");
            User user = new Models.User();
            if (userDetails != null)
            {
                user = JsonSerializer.Deserialize<User>(userDetails);
                ViewBag.userDetails = user;
            }
            return View();
        }
        public IActionResult AddTask()
        {
            try
            {
                var Priorities = _taskRepository.getPriorities();
                var Statues = _taskRepository.getStatuses();
                AddTaskViewModel addTaskViewModel = new AddTaskViewModel()
                {
                    Priorities = Priorities,
                    Statuses = Statues
                };
                

                return View(addTaskViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("LOG: AddTask action = " + ex.Message, DateTime.UtcNow);
                ViewBag.Error = ex.Message;
                throw;
            }
            
        }

        [HttpPost]
        public IActionResult AddTask(AddTaskViewModel addTaskViewModel)
        {
            if (addTaskViewModel.Task.PriorityId == 0)
            {
                ModelState.AddModelError("PriorityId", "Please Select Priority");
            }
            if (addTaskViewModel.Task.StatusId == 0)
            {
                ModelState.AddModelError("StatusId", "Please Select Status");
            }
            if (ModelState.IsValid) 
            {

            }
            else
            {
                var Priorities = _taskRepository.getPriorities();
                var Statues = _taskRepository.getStatuses();

                addTaskViewModel.Priorities = Priorities;
                addTaskViewModel.Statuses = Statues;
                
            }
            return View(addTaskViewModel);
        } 
    }
}

