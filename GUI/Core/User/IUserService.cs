using System.Collections.Generic;
using DTO;

namespace GUI.Core.UserService
{
    public interface IUserService
    {
        User Login(string username, string password);
        User CreateUser(string username, string password, Permission permission);
        List<User> GetAllUsers();
        void EditUser(User user);
        void DeleteUser(int id);
    }
}
