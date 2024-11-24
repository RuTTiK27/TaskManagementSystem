using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public byte RoleId { get; set; }

    public string ProfilePicture { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string? Pin { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
