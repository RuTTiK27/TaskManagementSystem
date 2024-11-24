using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UserVerifyViewModel
    {
        [Required]
        [MaxLength(6),MinLength(6)]
        public string? Pin { get; set; } = string.Empty;
    }
}
