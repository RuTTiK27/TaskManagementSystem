using Task = TaskManagementSystem.Models.Task;

namespace TaskManagementSystem.Repository
{
    public interface ITask
    {
        List<Task> getAllTasks();
        Task getTaskById(int id);
    }
}
