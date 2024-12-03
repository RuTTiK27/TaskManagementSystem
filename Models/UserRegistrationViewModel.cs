using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagementSystem.Models
{
    public class UserRegistrationViewModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        [MinLength(10, ErrorMessage = "Minimum Length Must be 10 characters")]
        [MaxLength(300, ErrorMessage = "Maximum Length is 300 characters")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Uppercase, Lowercase, Numbers, Symbols, Min 8 Chars")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not Mathcing With Password")]
        public string ConfirmPassword { get; set; }

        [ValidateNever]
        public IFormFile ProfilePicture { get; set; }
    }
}
