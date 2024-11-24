using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models;

public partial class Task
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

    public virtual User AssignedUser { get; set; } = null!;

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Priority Priority { get; set; } = null!;

    public virtual Statue Status { get; set; } = null!;
}
