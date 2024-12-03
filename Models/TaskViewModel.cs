using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TaskViewModel
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public byte PriorityId { get; set; }

        [Required]
        public byte StatusId { get; set; }

        public int AssignedUserId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; } = null;
    }
}
