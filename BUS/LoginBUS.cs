using BUS.Exceptions;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace BUS
{
    public class LoginBUS
    {
        private UserDAO _userDAO = new UserDAO();
        

        public User Login(string username, string password)
        {
            var user = _userDAO.GetUserByUsername(username);
            if (user == null || user.Password != password)
            {
                throw new CheckedException("Incorrect username or password!");
            }

            return user;

        }
    }
}
