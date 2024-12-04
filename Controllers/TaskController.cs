using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TaskManagementSystem.Filters;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;


namespace TaskManagementSystem.Controllers
{
    [AuthorizeSession]
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
            var userDetails = HttpContext.Session.GetString("UserDetails");
            User user = new Models.User();
            if (userDetails != null)
            {
                user = JsonSerializer.Deserialize<User>(userDetails);
                ViewBag.userDetails = user;
            }

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
            if (addTaskViewModel.PriorityId == 0)
            {
                ModelState.AddModelError("PriorityId", "Please Select Priority");
            }
            if (addTaskViewModel.StatusId == 0)
            {
                ModelState.AddModelError("StatusId", "Please Select Status");
            }
            var validExtensions = new[] { "jpg", "jpeg", "png","heic","pdf","docx","xlsx","doc","xls","mp4","mkv" };
            const long maxSize = 4 * 1024 * 1024; //4MB in bytes
            if (addTaskViewModel.Attachments.Count>4)
            {
                ModelState.AddModelError("Attachments", "More than 4 files are not allowed");
            }
            foreach (var file in addTaskViewModel.Attachments)
            {
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (validExtensions.Contains(fileExtension)) 
                    {
                        ModelState.AddModelError("Attachments", $"File {file.FileName} has an invalid file type.");
                        return View(addTaskViewModel);
                    }
                    if(file.Length > maxSize) 
                    {
                        ModelState.AddModelError("Attachments", $"File {file.FileName} exceeds the 8MB size limit.");
                        return View(addTaskViewModel);
                    }
                }
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

