using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
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

        public bool EmailAlreadyExists(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UsernameAlreadyExists(string username)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateIsActive(string email, string pin)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            
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
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
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

        public string GetPassword(string email)
        {
            var user = context.Users.FirstOrDefault(u=>u.Email.ToLower() == email.ToLower());
            if (user != null) 
            {
                return user.PasswordHash;
            }
            else
            {
                return null;
            }
        }
        
        public bool ValidUser(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null && user.IsActive==true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdatePassword(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                user.PasswordHash = password;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetUserProfile(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                return user.ProfilePicture;
            }
            else
            {
                return null;
            }
        }
    }
}
