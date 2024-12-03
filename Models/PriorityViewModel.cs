using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementSystem.Models
{
    public class PriorityViewModel
    {
        public byte PriorityId { get; set; }

        public string PriorityName { get; set; } = null!;
        public List<SelectListItem> Priorities { get; set; }
    }
}
