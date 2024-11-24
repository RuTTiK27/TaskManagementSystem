using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository
{
    public class UserRepository : IUser
    {
        private readonly TaskManagementSystemContext context;
        public UserRepository(TaskManagementSystemContext context)
        {
            this.context = context;
        }
        
        public bool AddUser(User users)
        {
            var entry = context.Users.Add(users);
            if (entry.State == EntityState.Added)
            {
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateIsActive(string email, string pin)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            
            if (user != null && user.Pin == pin)
            {
                user.IsActive = true;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdatePin(string email, string pin)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Pin = pin;
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
