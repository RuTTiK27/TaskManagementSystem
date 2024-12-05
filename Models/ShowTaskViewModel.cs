namespace TaskManagementSystem.Models
{
    public class ShowTaskViewModel
    {
        public int TaskId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public byte PriorityId { get; set; }  

        public byte StatusId { get; set; }

        public int AssignedUserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string randomColour { get; set; }
        // Navigation properties
        public Priority Priority { get; set; }
        public Statue Status { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public int AttachmentsCount { get; set; }
    }
}
