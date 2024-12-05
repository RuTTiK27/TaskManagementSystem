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

        List<Task> GetAllTask();

        List<Attachment> GetAttachments(int taskId);
    }
}
