
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Repository
{
    public class TaskRepository : ITask
    {
        private readonly TaskManagementSystemContext context;

        public TaskRepository(TaskManagementSystemContext context)
        {
            this.context = context;
        }
        public List<Models.Task> getAllTasks()
        {
            return context.Tasks.ToList();
        }

        public Models.Task getTaskById(int id)
        {
            return context.Tasks.ToList().Where(t=>t.TaskId == id).FirstOrDefault();
        }
    }
}
