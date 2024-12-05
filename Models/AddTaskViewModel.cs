using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [ValidateNever]
        public IEnumerable<SelectListItem> Priorities { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Statuses { get; set; }

        public byte PriorityId { get; set; }
        public byte StatusId { get; set; }

        [Required]
        [CustomValidation(typeof(DueDateValidator), nameof(DueDateValidator.ValidateDueDate))]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Now;

        [ValidateNever]
        public List<IFormFile> Attachments { get; set; }
    }
}
