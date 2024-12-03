
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

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

        public IEnumerable<SelectListItem> getPriorities()
        {
            var priorities = context.Priorities.Select(p=>new SelectListItem 
            {
                Value = p.PriorityId.ToString(),
                Text=p.PriorityName.ToString()
            }).ToList();
            priorities.Insert(0, new SelectListItem { Value = "0", Text = "Select Priority" });
            return priorities;
            //return context.Priorities.ToList();
        }

        public IEnumerable<SelectListItem> getStatuses()
        {
            var statuses = context.Statues.Select(s => new SelectListItem
            {
                Value = s.StatusId.ToString(),
                Text = s.StatusName.ToString()
            }).ToList();
            statuses.Insert(0, new SelectListItem { Value = "0", Text = "Select Status" });
            return statuses;
            //return context.Statues.ToList();
        }
    }
}
