using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using System.Data.Entity;


namespace DAO
{
    public class ProjectDAO
    {
        public List<Project> GetAllProjectsByMember(int userId)
        {
            using (var context = new ProjectManagementContext())
            {
                var projects = context.Projects.Include(p => p.CreatedUser).Include(p => p.Members).Include(p => p.Sprints).Include(p => p.Epics).Include(p => p.Epics.Select(e => e.UserStories)).Include(p => p.Sprints.Select(sprint => sprint.UserStories)).Include(p => p.Members.Select(member => member.User)).Include(p => p.Epics.Select(epic => epic.UserStories.Select(u => u.Tasks.Select(t => t.AssignedMember))));
                return projects.Where(p => p.Members.Any(m => m.User.Id == userId)).ToList();
            }
        }

        public void AddUserStoryToSprint(int sprintId, UserStory userStory)
        {
            using (var context = new ProjectManagementContext())
            {
                var sprint = context.Sprints.Include(s => s.UserStories).FirstOrDefault(s => s.Id == sprintId);
                var selectedUserStory = context.UserStories.Find(userStory.Id);
                context.UserStories.Attach(selectedUserStory);
                selectedUserStory.State = userStory.State;
                sprint.UserStories.Add(selectedUserStory);
                context.SaveChanges();
            }
        }

        public void AddEpic(int projectId, string epicTitle, string epicDescription)
        {
            using (var context = new ProjectManagementContext())
            {
                var project = context.Projects.Include(p => p.CreatedUser).Include(p => p.Members)
                    .Include(p => p.Epics).Include(p => p.Sprints).FirstOrDefault(p => p.Id == projectId);
                context.Projects.Attach(project);
                project.Epics.Add(new Epic()
                {
                    Title = epicTitle,
                    Description = epicDescription,
                    Project = project
                });
                context.SaveChanges();
            }
        }

        public List<User> GetAllUsersNotInProject(int projectId)
        {
            using (var context = new ProjectManagementContext())
            {
                var membersInProject = context.Projects.Include(project => project.Members).Include(project=> project.Members.Select(member => member.User)).FirstOrDefault(project => project.Id == projectId).Members.Select(member => member.User.Id);
                var users = context.Users.Where(user => !membersInProject.Any(id => id == user.Id)).ToList();
                return users;
            }
        }

        public bool CheckMember(int projectId, int userId)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Members.Any(member => member.Project.Id == projectId && member.User.Id == userId);
            }
        }

        public Project GetProjectByKey(string key)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Projects.FirstOrDefault(project => project.Key == key);
            }
        }

        public bool CheckKey(string key)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Projects.Any(project => project.Key == key);
            }
        }

        public void InsertProject(Project project)
        {
            using (var context = new ProjectManagementContext())
            {
                context.Users.Attach(project.CreatedUser);
                context.Projects.Add(project);
                context.SaveChanges();
            }
        }

        public void AddSprint(int projectId, Sprint sprint)
        {
            using (var context = new ProjectManagementContext())
            {
                var project = context.Projects.Include(p => p.CreatedUser).Include(p => p.Members)
                    .Include(p => p.Epics).Include(p => p.Sprints).FirstOrDefault(p => p.Id == projectId);
                context.Projects.Attach(project);
                sprint.UserStories = new System.Collections.ObjectModel.ObservableCollection<UserStory>();
                project.Sprints.Add(sprint);
                context.SaveChanges();
            }
        }

        public void StartSprint(int id, DateTime date)
        {
            using (var context = new ProjectManagementContext())
            {
                var sprint = context.Sprints.Find(id);
                sprint.State = SprintState.ACTIVE;
                sprint.EndDate = date;
                context.SaveChanges();
            }
        }

        public void ResetAssignedMember(int id)
        {
            using (var context = new ProjectManagementContext())
            {
                var task = context.Tasks.Include(t => t.AssignedMember).FirstOrDefault(t => t.Id == id);
                task.AssignedMember = null;
                context.SaveChanges();
            }
        }

        public void EndSprint(int id, DateTime date)
        {
            using (var context = new ProjectManagementContext())
            {
                var sprint = context.Sprints.Find(id);
                sprint.State = SprintState.FINISHED;
                sprint.EndDate = date;
                context.SaveChanges();
            }
        }

        public Member GetMemberById(int memberId)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Members.Include(m => m.User).Include(m => m.Project).FirstOrDefault(m => m.Id == memberId);
            }
        }

        public void AssignTask(int memberId, int taskId)
        {
            using (var context = new ProjectManagementContext())
            {
                var task = context.Tasks.Find(taskId);
                var member = context.Members.Find(memberId);
                context.Members.Attach(member);
                task.AssignedMember = member;
                context.SaveChanges();
            }
        }

        public void UpdateUserStoryState(int id, UserStoryState state)
        {
            using (var context = new ProjectManagementContext())
            {
                var userStory = context.UserStories.Find(id);
                userStory.State = state;
                context.SaveChanges();
            }
        }

        public UserStory GetUserStoryById(int id)
        {
            using (var context = new ProjectManagementContext())
            {
                var userStory = context.UserStories.Find(id);
                return userStory;
            }
        }

        public void AddTask(int userStoryId, Task task)
        {
            using (var context = new ProjectManagementContext())
            {
                var userStory = context.UserStories.Include(u => u.Tasks).FirstOrDefault(u => u.Id == userStoryId);
                userStory.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        public Project GetProjectById(int projectId)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Projects.Include(p => p.CreatedUser).Include(p => p.Members).Include(p => p.Sprints).Include(p => p.Epics).Include(p=>p.Epics.Select(e=>e.UserStories)).Include(p => p.Sprints.Select(sprint => sprint.UserStories)).Include(p=> p.Members.Select(member => member.User)).Include(p=>p.Epics.Select(epic => epic.UserStories.Select(u => u.Tasks))).FirstOrDefault(p => p.Id == projectId);
            }
        }

        public void EditProjectInfo(int id, string projectName, string projectKey)
        {
            using (var context = new ProjectManagementContext())
            {
                var project = context.Projects.Find(id);
                project.Name = projectName;
                project.Key = projectKey;
                context.SaveChanges();
            }
        }

        public Member GetMemberByUserId(int projectId, int userId)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Members.Include(member => member.User).Include(member => member.Project).FirstOrDefault(member => member.User.Id == userId && member.Project.Id == projectId);
            }
        }

        public void InsertMember(int id, Member member)
        {
            using (var context = new ProjectManagementContext())
            {
                var project = context.Projects.Include(p=> p.Members).FirstOrDefault(p => p.Id == id);
                var user = context.Users.Find(member.User.Id);
                context.Users.Attach(user);
                context.Projects.Attach(project);
                member.Project = project;
                member.User = user;

                project.Members.Add(member);
                context.SaveChanges();
            }
        }

        public void UpdateProject(Project project)
        {
            using (var context = new ProjectManagementContext())
            {
                var updatingProject = context.Projects.Find(project.Id);
                project.Key = updatingProject.Key;
                project.Members = updatingProject.Members;
                project.Name = updatingProject.Name;
                project.CreatedUser = updatingProject.CreatedUser;
                context.SaveChanges();
            }
        }

        public void DeleteProject(int projectId)
        {
            using (var context = new ProjectManagementContext())
            {
                var project = context.Projects.Find(projectId);
                context.Projects.Remove(project);
                context.SaveChanges();
            }
        }

        public void DeleteMember(int projectId, int memberId)
        {
            using (var context = new ProjectManagementContext())
            {
                var deletingMember = context.Members.Find(memberId);
                if(deletingMember != null)
                {
                    context.Members.Remove(deletingMember);
                    var memberProject = context.Projects.Include(project => project.Members).FirstOrDefault(project => project.Id == projectId);
                    if(memberProject != null)
                    {
                        deletingMember = memberProject.Members.FirstOrDefault(member => member.Id == memberId);
                        memberProject.Members.Remove(deletingMember);
                    }
                    context.SaveChanges();
                }
            }
        }

        public void UpdateMemberRole(int memberId, Role role)
        {
            using (var context = new ProjectManagementContext())
            {
                var updatingMember = context.Members.Find(memberId);
                updatingMember.Role = role;
                context.SaveChanges();
            }
        }
    }
}
