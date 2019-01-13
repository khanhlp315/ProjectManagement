using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public enum Permission 
    {
        [Description("Owner")]
        OWNER = 0,
        [Description("Admin")]
        ADMIN = 1,
        [Description("Member")]
        MEMBER = 2
    }
}
