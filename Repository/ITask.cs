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
    }
}
