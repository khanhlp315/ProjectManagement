using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUS;
using BUS.Exceptions;
using DTO;

namespace GUI.Core.UserService
{
    public class UserService : IUserService
    {
        private readonly LoginBUS _userBus = new LoginBUS();
        private readonly TeamBUS _teamBus = new TeamBUS();
        public User Login(string username, string password)
        {
            try
            {
                var user = _userBus.Login(username, password);
                return user;
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return _teamBus.GetAllUsers();
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        private TeamBUS _teamBUS = new TeamBUS();
        public User CreateUser(string username, string password, Permission permission)
        {
            try
            {
                var user = _teamBUS.CreateUser(username, password, permission);
                return user;
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public void EditUser(User user)
        {
            try
            {
                _teamBUS.EditUser(user);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                 _teamBUS.DeleteUser(id);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }
    }

}
