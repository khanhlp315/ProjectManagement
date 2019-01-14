using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using System.Data.Entity;

namespace DAO
{
    public class UserDAO
    {
        public User GetUserByUsername(string username)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Users.FirstOrDefault(user => user.Username == username);
            }
        }

        public bool CheckUsername(string username)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Users.Any(user => user.Username == username);
            }
        }

        public List<User> GetAllUsers()
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Users.ToList();
            }
        }

        public User GetUserById(int id)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Users.Find(id);
            }
        }

        public void InsertUser(User user)
        {
            using (var context = new ProjectManagementContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public void UpdateUser(User updatingUser)
        {
            using (var context = new ProjectManagementContext())
            {
                var user = context.Users.Find(updatingUser.Id);
                user.Username = updatingUser.Username;
                user.Password = updatingUser.Password;
                user.Permission = updatingUser.Permission;                
                context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (var context = new ProjectManagementContext())
            {
                var user = context.Users.Find(id);
                if(user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }
        }

        public User GetUserByMemberId(int memberId)
        {
            using (var context = new ProjectManagementContext())
            {
                var user = context.Members.Include(m => m.User).FirstOrDefault(m => m.Id == memberId).User;
                return user;
            }
        }
    }
}
