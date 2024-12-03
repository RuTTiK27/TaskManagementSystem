namespace TaskManagementSystem.Models
{
    public class AttatchmentViewModel
    {
        public string FileName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public string FileType { get; set; } = null!;

        public int FileSize { get; set; }

        public bool? IsDeleted { get; set; }

        public int TaskId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; } = null;
    }
}
