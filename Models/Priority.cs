using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models;

public partial class Priority
{
    public byte PriorityId { get; set; }

    public string PriorityName { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
