using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UserForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
