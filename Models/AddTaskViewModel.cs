using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class AddTaskViewModel
    {
        public TaskViewModel Task { get; set; }
        public IEnumerable<SelectListItem> Priorities { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }

    }
}
