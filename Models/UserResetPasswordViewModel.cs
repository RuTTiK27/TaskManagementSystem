using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UserResetPasswordViewModel
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Uppercase, Lowercase, Numbers, Symbols, Min 8 Chars")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not Mathcing With Password")]
        public string ConfirmPassword { get; set; }
    }
}
