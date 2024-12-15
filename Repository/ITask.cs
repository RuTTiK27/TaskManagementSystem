using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Models;
using Task = TaskManagementSystem.Models.Task;

namespace TaskManagementSystem.Repository
{
    public interface ITask
    {
        List<Task> getAllTasks();
        Task getTaskById(int id);
        IEnumerable<SelectListItem> getPriorities();
        IEnumerable<SelectListItem> getStatuses();
        int AddTask(Task task);
        bool AddAttachment(Attachment attachment);
        public void saveAttachments();

        List<Task> GetAllTask(int userId);

        List<Attachment> GetAttachments(int taskId);
        Task EditTask(int taskId);

        int UpdateTask(Task task);

        bool DeleteAttachment(int attachmentId);
        bool DeleteTask(int taskId);
    }
}
