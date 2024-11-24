using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models;

public partial class Statue
{
    public byte StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
