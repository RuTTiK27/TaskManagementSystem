using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUser _userRepository;
        private readonly FileUploadService _fileUploadService;
        private readonly SendVerifyEmailService _sendVerifyEmailService;
        
        public UserController(FileUploadService fileUploadService,ILogger<UserController> logger,IUser userRepository,SendVerifyEmailService sendVerifyEmailService)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
            _userRepository = userRepository;
            _sendVerifyEmailService = sendVerifyEmailService;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserRegistrationViewModel userRegistrationViewModel) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filename = "";
                    if (userRegistrationViewModel.ProfilePicture == null) { filename = "Images/TMSUser.png"; }
                    else
                    {
                        filename = _fileUploadService.UploadProfilePicture(userRegistrationViewModel.ProfilePicture);
                        if (filename.Equals("sizeError"))
                        {
                            TempData["sizeError"] = "Image must be less than 2 MB";
                        }
                        else if (filename.Equals("ExtensionError"))
                        {
                            TempData["ExtensionError"] = "Only PNG, JPG, JPEG images allowed.";
                        }
                    }

                    if (!filename.Equals("sizeError") || !filename.Equals("ExtensionError"))
                    {
                        User user = new User()
                        {
                            FirstName = userRegistrationViewModel.FirstName,
                            LastName = userRegistrationViewModel.LastName,
                            UserName = userRegistrationViewModel.Username,
                            Email = userRegistrationViewModel.Email,
                            PasswordHash = HashPassword(userRegistrationViewModel.Password),
                            ProfilePicture = filename,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = null,
                            LastLoginDate = null,
                            RoleId = 1,
                            IsActive = false,
                            Pin = null
                        };
                        var status = _userRepository.AddUser(user);
                        if (status)
                        {
                            HttpContext.Session.SetString("Email", userRegistrationViewModel.Email);
                            return RedirectToAction("VerifyUser", "User");
                        }
                    }
                }
                return View(userRegistrationViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("LOG: Registraton action = " + ex.Message, DateTime.UtcNow);
                ViewBag.Error = ex.Message;
                throw;
            }
        }
        private string HashPassword(string password) 
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public IActionResult VerifyUser(string Email) 
        {
            try
            {
                var email = HttpContext.Session.GetString("Email");
                string pin = _sendVerifyEmailService.GenerateVerificationCode();
                if (email != null)
                {
                    _userRepository.UpdatePin(email, pin);
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("LOG: Registraton action = " + ex.Message, DateTime.UtcNow);
                ViewBag.Error = ex.Message;
                throw;
            }
            
        }

        [HttpPost]
        public IActionResult VerifyUser(UserVerifyViewModel userVerifyViewModel) 
        {
            try
            {
                var email = HttpContext.Session.GetString("Email");
                if (ModelState.IsValid)
                {
                    if (email !=null && userVerifyViewModel.Pin!=null) 
                    {
                        var isValidPin = _userRepository.UpdateIsActive(email, userVerifyViewModel.Pin);
                        if (isValidPin) 
                        {
                            return RedirectToAction("UserDashboard", "Task");
                        }
                        else
                        {
                            TempData["incorrectPin"] = "Entered PIN is incorrect";
                        }
                    }
                }
                return View(userVerifyViewModel);
            }
            catch (Exception ex) 
            {
                _logger.LogInformation("LOG: Registraton action = " + ex.Message, DateTime.UtcNow);
                ViewBag.Error = ex.Message;
                throw;
            }
        }
    }
}
