using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Internal;
using System.Data.Entity.Utilities;
using System.Data.Entity;
using DTO;

namespace DAO
{
    public class DatabaseIntializer: DropCreateDatabaseIfModelChanges<ProjectManagementContext>
    {
        protected override void Seed(ProjectManagementContext context)
        {
            context.Users.Add(new User
            {
                Username = "admin",
                Password = "admin",
                Permission = Permission.ADMIN
            });
            base.Seed(context);
        }

    }
}
