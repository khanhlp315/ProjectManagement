using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace GUI.AppInitializer.Stores
{
    public interface IStore
    {
        void SetCurrentUser(User user);
        User GetCurrentUser();
        bool IsLoggedIn();
    }
}
