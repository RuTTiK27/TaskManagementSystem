using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementSystem.Filters;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;
using Task = TaskManagementSystem.Models.Task;


namespace TaskManagementSystem.Controllers
{
    [AuthorizeSession]
    public class TaskController : Controller
    {
        private readonly ITask _taskRepository; 
        private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _environment;

        public TaskController(ITask taskRepository, ILogger<UserController> logger, IWebHostEnvironment environment)
        {
                _taskRepository = taskRepository;
                _logger = logger;
                _logger = logger;
                _environment = environment;
        }
        public IActionResult UserDashboard(string? sortBy = null,string? search=null,DateTime? dueDate=null,string? priority=null,string? status=null)
        {
            var userDetails = HttpContext.Session.GetString("UserDetails");
            User user = new Models.User();
            if (userDetails != null)
            {
                user = JsonSerializer.Deserialize<User>(userDetails);
                ViewBag.userDetails = user;
            }

            string[] colorPalette = {
            "#007bff", "#0056b3", "#3399ff", "#0044cc", "#0066cc", // Blue Shades
            "#e83e8c", "#d63384", "#f062a5", "#c2185b", "#ff4d94", "#e63985", // Pink Shades
            "#6f42c1", "#5a34a6", "#7b4ec7", "#5e2eab", "#8e44d4", "#7648c9" // Purple Shades
            };
            var random = new Random();

            List<ShowTaskViewModel> showTaskViewModels = new List<ShowTaskViewModel>();
            var tasks = _taskRepository.GetAllTask(user.UserId);
            var query = tasks.AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (dueDate!=null)
            {
                query = query.Where(t => t.DueDate.Date == dueDate);
            }

            if (!String.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status.StatusName.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrEmpty(priority))
            {
                query = query.Where(t => t.Priority.PriorityName.Equals(priority, StringComparison.OrdinalIgnoreCase));
            }

            if (sortBy!=null)
            {
                switch (sortBy.ToLower())
                {
                    case "title":
                        query = query.OrderBy(x => x.Title);
                        break;
                    case "lastadded":
                        query = query.OrderByDescending(x => x.CreatedDate);
                        break;
                    default:
                        break;
                }
            }
            foreach (var task in query) 
            {
                showTaskViewModels.Add(new ShowTaskViewModel
                {
                    TaskId = task.TaskId,
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    PriorityName = task.Priority.PriorityName,
                    StatusName = task.Status.StatusName,
                    randomColour = colorPalette[random.Next(colorPalette.Length)],
                    AttachmentsCount = task.Attachments.Count
                });
            }

            return View(showTaskViewModels);
        }
        
        [HttpGet]
        public IActionResult GetAttachments(int taskId) 
        {
            var attachments = _taskRepository.GetAttachments(taskId);
            if (attachments == null )
            {
                return Json(new { success = false, message = "No attachments found." });
            }
            var result = attachments.Select(a => new
            {
                attachmentId = a.AttachmentId,
                fileName = a.FileName,
                filePath = Url.Content($"~/Attachments/{Path.GetFileName(a.FilePath)}"), // Convert to URL-safe path
                fileType = a.FileType,
                fileSize = a.FileSize
            });

            return Json(new { success = true, data = result });
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

            if (addTaskViewModel.Attachments != null) 
            {
                var validExtensions = new[] { "jpg", "jpeg", "png", "heic", "pdf", "docx", "xlsx", "doc", "xls", "mp4", "mkv" };
                const long maxSize = 4 * 1024 * 1024; //4MB in bytes
                if (addTaskViewModel.Attachments.Count > 4)
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
                        if (file.Length > maxSize)
                        {
                            ModelState.AddModelError("Attachments", $"File {file.FileName} exceeds the 8MB size limit.");
                            return View(addTaskViewModel);
                        }
                    }
                }
            }

            var userDetails = HttpContext.Session.GetString("UserDetails");
            User user = new Models.User();
            if (userDetails != null)
            {
                user = JsonSerializer.Deserialize<User>(userDetails);
            }

            if (ModelState.IsValid) 
            {
                Task task = new Task()
                {
                    Title = addTaskViewModel.Title,
                    Description = addTaskViewModel.Description,
                    DueDate = addTaskViewModel.DueDate,
                    PriorityId = addTaskViewModel.PriorityId,
                    StatusId = addTaskViewModel.StatusId,
                    AssignedUserId = user.UserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null
                };
                var taskId = _taskRepository.AddTask(task);
                if (addTaskViewModel.Attachments != null)
                {
                    foreach (var file in addTaskViewModel.Attachments)
                    {
                        string folder = Path.Combine(_environment.WebRootPath, "Attachments/");
                        string filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(folder, filename);

                        using (var fileStram = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStram);
                        }
                        Attachment attachment = new Attachment()
                        {
                            FileName = file.FileName,
                            FilePath = filePath,
                            FileType = Path.GetExtension(file.FileName).ToLower(),
                            FileSize = Convert.ToInt32(file.Length / (1024.0 * 1024.0)),
                            IsDeleted = false,
                            TaskId = taskId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = null,
                        };
                        _taskRepository.AddAttachment(attachment);
                    }
                    _taskRepository.saveAttachments();
                }
                TempData["ShowToast"] = "Yes";
                TempData["toastMessage"] = "Task Added";
                return RedirectToAction("UserDashboard","Task");
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

        public IActionResult UpdateTask(int TaskId) 
        {
            TempData["TaskId"] = TaskId;
            TempData.Keep();
            var task = _taskRepository.EditTask(TaskId);
            var Priorities = _taskRepository.getPriorities();
            var Statues = _taskRepository.getStatuses();
            AddTaskViewModel addTaskViewModel = new AddTaskViewModel()
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                PriorityId = task.PriorityId,
                StatusId = task.StatusId,
                HasAttachments = task.Attachments.ToList(),
                Priorities = Priorities,
                Statuses = Statues
            };
            return View(addTaskViewModel);
        }

        [HttpPost]
        public IActionResult UpdateTask(AddTaskViewModel addTaskViewModel)
        {
            if (addTaskViewModel.PriorityId == 0)
            {
                ModelState.AddModelError("PriorityId", "Please Select Priority");
            }
            if (addTaskViewModel.StatusId == 0)
            {
                ModelState.AddModelError("StatusId", "Please Select Status");
            }

            if (addTaskViewModel.Attachments != null)
            {
                var validExtensions = new[] { "jpg", "jpeg", "png", "heic", "pdf", "docx", "xlsx", "doc", "xls", "mp4", "mkv" };
                const long maxSize = 4 * 1024 * 1024; //4MB in bytes
                if (addTaskViewModel.Attachments.Count > 4)
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
                        if (file.Length > maxSize)
                        {
                            ModelState.AddModelError("Attachments", $"File {file.FileName} exceeds the 8MB size limit.");
                            return View(addTaskViewModel);
                        }
                    }
                }
            }

            var userDetails = HttpContext.Session.GetString("UserDetails");
            User user = new Models.User();
            if (userDetails != null)
            {
                user = JsonSerializer.Deserialize<User>(userDetails);
            }

            if (ModelState.IsValid)
            {
                Task task = new Task()
                {
                    TaskId = Convert.ToInt32(TempData["TaskId"].ToString()),
                    Title = addTaskViewModel.Title,
                    Description = addTaskViewModel.Description,
                    DueDate = addTaskViewModel.DueDate,
                    PriorityId = addTaskViewModel.PriorityId,
                    StatusId = addTaskViewModel.StatusId,
                    AssignedUserId = user.UserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null
                };
                var taskId = _taskRepository.UpdateTask(task);
                if (addTaskViewModel.Attachments != null)
                {
                    foreach (var file in addTaskViewModel.Attachments)
                    {
                        string folder = Path.Combine(_environment.WebRootPath, "Attachments/");
                        string filename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(folder, filename);

                        using (var fileStram = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStram);
                        }
                        Attachment attachment = new Attachment()
                        {
                            FileName = file.FileName,
                            FilePath = filePath,
                            FileType = Path.GetExtension(file.FileName).ToLower(),
                            FileSize = Convert.ToInt32(file.Length / (1024.0 * 1024.0)),
                            IsDeleted = false,
                            TaskId = taskId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = null,
                        };
                        _taskRepository.AddAttachment(attachment);
                    }
                    _taskRepository.saveAttachments();
                }
                TempData["ShowToast"] = "Yes";
                TempData["toastMessage"] = "Task "+taskId+" Updated";
                return RedirectToAction("UserDashboard", "Task");
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

        public IActionResult DeleteAttatchment(int AttachmentId) 
        {
            var TaskId = TempData["TaskId"].ToString();

            var result = _taskRepository.DeleteAttachment(AttachmentId);
            return RedirectToAction("UpdateTask","Task", new { TaskId = TaskId });
        }
        public IActionResult DeleteTask(int taskId)
        {
            var isDeleted = _taskRepository.DeleteTask(taskId);
            if (isDeleted)
            {
                TempData["ShowToast"] = "Yes";
                TempData["toastMessage"] = "Task " + taskId + " Deleted";
            }
            else
            {
                TempData["ShowToast"] = "Yes";
                TempData["toastMessage"] = "Task " + taskId + " NOT Deleted";
            }
            return RedirectToAction("UserDashboard", "Task");
        }
    }
}

