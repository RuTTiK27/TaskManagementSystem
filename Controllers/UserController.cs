﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text.Json;
using System.Threading.Tasks;
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
        [HttpPost]
        public IActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userRepository.EmailAlreadyExists(userLoginViewModel.Email)) 
                    {
                        var passwordHashed = _userRepository.GetPassword(userLoginViewModel.Email);
                        if(VerifyPassword(userLoginViewModel.Password, passwordHashed))
                        {
                            setSesson(userLoginViewModel.Email, false);
                            if (_userRepository.ValidUser(userLoginViewModel.Email))
                            {
                                setSesson(userLoginViewModel.Email, true);
                                return RedirectToAction("UserDashboard", "Task");
                            }
                            else
                            {
                                return RedirectToAction("VerifyUser", "User");
                            }
                        }
                        else 
                        {
                            TempData["incorrectPassword"] = true;
                        }
                    }
                    else
                    {
                        TempData["EmailNotExists"] = true;
                    }
                }
                return View(userLoginViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("LOG: Registraton action = " + ex.Message, DateTime.UtcNow);
                ViewBag.Error = ex.Message;
                throw;
            }
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
                    if (_userRepository.EmailAlreadyExists(userRegistrationViewModel.Email))
                    {
                        TempData["emailAlreadyExists"] = true;
                    }
                    else if (_userRepository.UsernameAlreadyExists(userRegistrationViewModel.Username))
                    {
                        TempData["usernameAlreadyExists"] = true;
                    }
                    else
                    {
                        var filename = "";
                        if (userRegistrationViewModel.ProfilePicture == null) { filename = "TMSUser.png"; }
                        else
                        {
                            filename = _fileUploadService.UploadProfilePicture(userRegistrationViewModel.ProfilePicture);
                            if (filename.Equals("SizeError"))
                            {
                                TempData["SizeError"] = "Image must be less than 2 MB";
                            }
                            else if (filename.Equals("ExtensionError"))
                            {
                                TempData["ExtensionError"] = "Only PNG, JPG, JPEG images allowed.";
                            }
                        }

                        if (filename.Equals("SizeError"))
                        {
                            return View(userRegistrationViewModel);
                        }
                        else if (filename.Equals("ExtensionError"))
                        {
                            return View(userRegistrationViewModel);
                        }
                        else 
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
                                setSesson(userRegistrationViewModel.Email, false);
                                return RedirectToAction("VerifyUser", "User");
                            }
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

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }

        public IActionResult VerifyUser(bool fromForgotPassword) 
        {
            try
            {
                if (fromForgotPassword) 
                {
                    TempData["fromForgotPassword"] = "Yes";
                }

                var email = "";                
                var userDetails = HttpContext.Session.GetString("UserDetails");
                User user = new Models.User();
                if (userDetails != null)
                {
                    user = JsonSerializer.Deserialize<User>(userDetails);
                    email = user.Email;
                }

                string pin = _sendVerifyEmailService.GenerateVerificationCode();
                if (email != null)
                {
                    var isPinUpdated = _userRepository.UpdatePin(email, pin);
                    if (isPinUpdated)
                    {
                        _sendVerifyEmailService.SendVerificationEmailAsync(email, pin);
                    }
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
                var email = "";
                var userDetails = HttpContext.Session.GetString("UserDetails");
                User user = new Models.User();
                if (userDetails != null)
                {
                    user = JsonSerializer.Deserialize<User>(userDetails);
                    email = user.Email;
                }

                if (ModelState.IsValid)
                {
                    if (email != null && userVerifyViewModel.Pin != null)
                    {
                        var isValidPin = _userRepository.UpdateIsActive(email, userVerifyViewModel.Pin);
                        if (TempData["fromForgotPassword"]!=null)
                        {
                            if (isValidPin && TempData["fromForgotPassword"].ToString().Equals("Yes"))
                            {
                                return RedirectToAction("ResetPassword", "User");
                            }
                        }
                        else if (isValidPin) 
                        {
                            setSesson(email, true);
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

        [HttpPost]
        public bool ResendVerification()
        {
            try
            {
                var email = "";
                var userDetails = HttpContext.Session.GetString("UserDetails");
                User user = new Models.User();
                if (userDetails != null)
                {
                    user = JsonSerializer.Deserialize<User>(userDetails);
                    email = user.Email;
                }

                string pin = _sendVerifyEmailService.GenerateVerificationCode();
                if (email != null)
                {
                    var isPinUpdated = _userRepository.UpdatePin(email, pin);
                    if (isPinUpdated)
                    {
                        _sendVerifyEmailService.SendVerificationEmailAsync(email, pin);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("LOG: Registraton action = " + ex.Message, DateTime.UtcNow);
                ViewBag.Error = ex.Message;
                throw;
            }
        }

        public IActionResult ForgotPassword() 
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult ForgotPassword(UserForgotViewModel userForgotViewModel) 
        {
            if (_userRepository.EmailAlreadyExists(userForgotViewModel.Email))
            {
                setSesson(userForgotViewModel.Email, false);
                return RedirectToAction("VerifyUser", "User", new { fromForgotPassword = true });
            }
            else
                TempData["EmailNotExists"] = true;
            
            return View(userForgotViewModel);
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(UserResetPasswordViewModel userResetPasswordViewModel)
        {
            if (ModelState.IsValid) 
            {
                var email = "";
                var userDetails = HttpContext.Session.GetString("UserDetails");
                User user = new Models.User();
                if (userDetails != null)
                {
                    user = JsonSerializer.Deserialize<User>(userDetails);
                    email = user.Email;
                }

                if (_userRepository.UpdatePassword(email, HashPassword(userResetPasswordViewModel.Password)))
                {
                    setSesson(email, true);
                    TempData["ShowToast"] = "Yes";
                    TempData["toastMessage"] = "Password Updated";
                    return RedirectToAction("UserDashboard", "Task");
                }
            }
            return View(userResetPasswordViewModel);
        }

        private void setSesson(string email,bool afterlogin) 
        {
            if (afterlogin)
            {
                var user = _userRepository.getUserDetails(email);
                HttpContext.Session.SetString("UserDetails", JsonSerializer.Serialize(new
                {
                    Email = user.Email,
                    ProfilePicture = user.ProfilePicture,
                    UserId = user.UserId
                }));
            }
            else 
            {
                HttpContext.Session.SetString("UserDetails", JsonSerializer.Serialize(new
                {
                    Email = email,
                }));
            }
        }

    }
}
