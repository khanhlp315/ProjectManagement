using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Text.RegularExpressions;
using BUS.Exceptions;

namespace BUS
{
    public class TeamBUS
    {
        private UserDAO _userDAO = new UserDAO();

        public List<User> GetAllUsers()
        {
            return _userDAO.GetAllUsers();
        }

        public User CreateUser(string username, string password, Permission permission)
        {
            if (!Regex.IsMatch(username, "[A-Za-z0-9]{1,10}"))
            {
                throw new CheckedException("Username must contains only alphabetical letters or digits, and must have at least 1 and at most 10 characters.");
            }
            if (!Regex.IsMatch(password, "[A-Za-z0-9]{3,15}"))
            {
                throw new CheckedException("Password must contains only alphabetical letters or digits, and must have at least 3 and at most 15 characters.");
            }

            if (_userDAO.CheckUsername(username))
            {
                throw new CheckedException($"Username {username} has already been used.");
            }

            User user = new User
            {
                Username = username,
                Permission = permission,
                Password = password
            };
            _userDAO.InsertUser(user);

            user = _userDAO.GetUserByUsername(username);
            return user;
        }

        public void EditUser(User user)
        {
            if (!Regex.IsMatch(user.Username, "[A-Za-z0-9]{1,10}"))
            {
                throw new CheckedException("Username must contains only alphabetical letters or digits, and must have at least 1 and at most 10 characters.");
            }
            if (!Regex.IsMatch(user.Password, "[A-Za-z0-9]{3,15}"))
            {
                throw new CheckedException("Password must contains only alphabetical letters or digits, and must have at least 3 and at most 15 characters.");
            }

            var userWithUsername = _userDAO.GetUserByUsername(user.Username);
            if (userWithUsername != null && userWithUsername.Id != user.Id)
            {
                throw new CheckedException($"Username {user.Username} has already been used.");
            }
            
            _userDAO.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            _userDAO.DeleteUser(userId);
        }

        public User GetUserFromMember(int memberId)
        {
            return _userDAO.GetUserByMemberId(memberId);
        }
    }
}
