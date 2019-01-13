using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public enum Role
    {
        [Description("Owner")]
        OWNER = 0,
        [Description("Product Owner")]
        PRODUCT_OWNER = 1,
        [Description("Scrum master")]
        SCRUM_MASTER = 2,
        [Description("Developer")]
        DEVELOPER = 3
    }
}
