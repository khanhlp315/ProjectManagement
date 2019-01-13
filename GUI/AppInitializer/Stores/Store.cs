using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace GUI.AppInitializer.Stores
{
    public class Store: IStore
    {
        private User _user;

        public User GetCurrentUser()
        {
            return _user;
        }

        public bool IsLoggedIn()
        {
            return _user != null;
        }

        public void SetCurrentUser(User user)
        {
            _user = user;
        }
    }
}
