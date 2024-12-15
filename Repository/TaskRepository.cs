
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using Task = TaskManagementSystem.Models.Task;
using Attachment = TaskManagementSystem.Models.Attachment;

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

        public List<Task> GetAllTask(int userId)
        {
            return context.Tasks
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t=>t.Attachments)
            .Where(t => t.AssignedUserId == userId).ToList();
        }

        public List<Attachment> GetAttachments(int taskId)
        {
            return context.Attachments.Where(t => t.TaskId == taskId && t.IsDeleted==false).ToList();
        }

        public Task EditTask(int taskId)
        {
            return context.Tasks.Include(t => t.Attachments).Where(t => t.TaskId == taskId).FirstOrDefault();
        }
        
        public bool DeleteAttachment(int attachmentId)
        {
            var attachment = context.Attachments.FirstOrDefault(a=>a.AttachmentId == attachmentId);
            if (attachment != null)
            {
                attachment.IsDeleted = true;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public int UpdateTask(Task task)
        {
            var taskFromDB = context.Tasks.FirstOrDefault(t => t.TaskId == task.TaskId);

            if (taskFromDB != null)
            {
                taskFromDB.Title = task.Title;
                taskFromDB.Description = task.Description;
                taskFromDB.DueDate = task.DueDate;
                taskFromDB.PriorityId = task.PriorityId;
                taskFromDB.StatusId = task.StatusId;
                taskFromDB.UpdatedDate = DateTime.Now;
                context.SaveChanges();
                return task.TaskId;
            }
            else
            {
                return 0;
            }
        }

        public bool DeleteTask(int taskId)
        {
            var task = context.Tasks.Include(t=>t.Attachments).FirstOrDefault(t => t.TaskId == taskId);
            if (task != null)
            {
                foreach (var attachment in task.Attachments)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath);
                    if (File.Exists(filePath)) 
                    {
                        File.Delete(filePath);
                    }
                }
                context.Tasks.Remove(task);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
