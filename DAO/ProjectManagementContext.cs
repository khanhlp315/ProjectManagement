using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DAO
{
    public class ProjectManagementContext: DbContext
    {
        public ProjectManagementContext(): base("ProjectManagementDB")
        {
            Database.SetInitializer(new DatabaseIntializer());
        }

        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<Project> Projects
        {
            get;
            set;
        }
        public DbSet<Member> Members
        {
            get;
            set;
        }
        public DbSet<Sprint> Sprints
        {
            get;
            set;
        }
        public DbSet<Epic> Epics
        {
            get;
            set;
        }
        public DbSet<UserStory> UserStories
        {
            get;
            set;
        }
        public DbSet<Task> Tasks
        {
            get;
            set;
        }
    }
}
