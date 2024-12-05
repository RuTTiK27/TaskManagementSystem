
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using Task = TaskManagementSystem.Models.Task;

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
            return context.Tasks.ToList().Where(t => t.TaskId == id).FirstOrDefault();
        }

        public IEnumerable<SelectListItem> getPriorities()
        {
            var priorities = context.Priorities.Select(p => new SelectListItem
            {
                Value = p.PriorityId.ToString(),
                Text = p.PriorityName.ToString()
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

        public int AddTask(Task task)
        {
            var entry = context.Tasks.Add(task);
            if (entry.State == EntityState.Added)
            {
                context.SaveChanges();
                return task.TaskId;
            }
            else
            {
                return 0;
            }

        }

        public bool AddAttachment(Attachment attachment)
        {
            var entry = context.Attachments.Add(attachment);
            if (entry.State == EntityState.Added)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void saveAttachments() 
        {
            context.SaveChanges();
        }

        public List<Task> GetAllTask()
        {
            return context.Tasks
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t=>t.Attachments)
            .ToList();
        }

        public List<Attachment> GetAttachments(int taskId)
        {
            var attachments = context.Attachments.Where(t => t.TaskId == taskId).ToList();
            var result = attachments.Select(a => new
            {
                id = a.AttachmentId,
                fileName = a.FileName,
                fileType = a.FileType,
                downloadUrl = Url.Content($"~/Attachments/{a.FileName}")
            });
            return result;
        }
    }
}
