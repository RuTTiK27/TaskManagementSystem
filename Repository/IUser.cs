using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository
{
    public interface IUser
    {
        bool AddUser(User users);
        bool UpdatePin(string email,string pin);
        bool UpdateIsActive(string email, string pin);
        bool EmailAlreadyExists(string email);
        bool UsernameAlreadyExists(string username);
    }
}
