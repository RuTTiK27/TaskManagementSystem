using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.CustomValidators;

namespace TaskManagementSystem.Models
{
    public class AddTaskViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = null!;

        public IEnumerable<SelectListItem> Priorities { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }

        public int PriorityId { get; set; }
        public int StatusId { get; set; }

        [Required]
        [CustomValidation(typeof(DueDateValidator), nameof(DueDateValidator.ValidateDueDate))]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [Required]
        public List<IFormFile> Attachments { get; set; }
    }
}
