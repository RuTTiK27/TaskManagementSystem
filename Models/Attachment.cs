using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models;

public partial class Attachment
{
    public int AttachmentId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public int FileSize { get; set; }

    public bool? IsDeleted { get; set; }

    public int TaskId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Task Task { get; set; } = null!;
}
