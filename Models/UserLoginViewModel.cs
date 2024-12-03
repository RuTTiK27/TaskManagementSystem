using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UserLoginViewModel
    {
        [Required]
        [MaxLength(300)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
