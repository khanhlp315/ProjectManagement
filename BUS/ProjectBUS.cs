using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BUS.Exceptions;
using DAO;
using DTO;

namespace BUS
{
    public class ProjectBUS
    {
        private UserDAO _userDAO = new UserDAO();
        private ProjectDAO _projectDAO = new ProjectDAO();
        private EpicDAO _epicDAO = new EpicDAO();

        public Epic AddEpic(int projectId, string epicTitle, string epicDescription)
        {
            if(_epicDAO.CheckTitle(projectId, epicTitle))
            {
                throw new CheckedException($"Epic {epicTitle} already exist.");
            }
            _projectDAO.AddEpic(projectId, epicTitle, epicDescription);
            return _epicDAO.GetEpicByTitle(projectId, epicTitle);
        }

        public void AddUserStoryToSprint(int sprintId, UserStory userStory)
        {
            if(userStory.State != UserStoryState.BACKLOG)
            {
                throw new CheckedException($"User story {userStory.Title} is already finished.");
            }
            userStory.State = UserStoryState.ON_SPRINT;
            _projectDAO.AddUserStoryToSprint(sprintId, userStory);
        }

        public void AddUserStory(int epicId, string title, string description, int storyPoints)
        {
            if (storyPoints <= 0)
            {
                throw new CheckedException("Story points must be a positive number.");
            }

            if (_epicDAO.CheckUserStory(epicId, title))
            {
                throw new CheckedException($"User story {title} has already been created.");
            }

            _epicDAO.AddUserStory(epicId, title, description, storyPoints);
        }

        public void CreateProject(int userId, string key, string name)
        {
            if(!Regex.IsMatch(key, "\\b[A-Z0-9]{2,5}\\b"))
            {
                throw new CheckedException("Key must contains only uppercase alphabetical letters or digits, and must have at least 2 and at most 5 characters.");
            }

            if(_projectDAO.CheckKey(key))
            {
                throw new CheckedException($"Key {key} has already been used.");
            }

            var user = _userDAO.GetUserById(userId);

            if(user == null)
            {
                throw new CheckedException("User does not exist");
            }

            if(user.Permission != Permission.OWNER)
            {
                throw new CheckedException("You don't have permission to create a project");
            }

            Project project = new Project
            {
                Key = key,
                Name = name,
                CreatedUser = _userDAO.GetUserById(userId),
                Members = new ObservableCollection<Member>()
            };
            _projectDAO.InsertProject(project);

            Member member = new Member
            {
                User = user,
                Role = Role.OWNER
            };
            _projectDAO.InsertMember(project.Id, member);
        }

        public Sprint GetNextSprint(int projectId)
        {
            var project = _projectDAO.GetProjectById(projectId);

            if(project.Sprints.Count == 0)
            {
                var sprint = new Sprint()
                {
                    Order = 0,
                    State = SprintState.QUEUING,
                };
                _projectDAO.AddSprint(projectId, sprint);
                project = _projectDAO.GetProjectById(projectId);
            }

            return project.Sprints.OrderByDescending(sprint => sprint.Order).First();
        }

        public void StartSprint(Sprint sprint)
        {
            if (sprint.State != SprintState.QUEUING)
            {
                throw new CheckedException($"Sprint has already stảted.");

            }
            var date = DateTime.Today;
            _projectDAO.StartSprint(sprint.Id, date);
            foreach (var userStory in sprint.UserStories)
            {
                _projectDAO.UpdateUserStoryState(userStory.Id, UserStoryState.IN_PROGRESS);
            }
        }

        public Sprint EndSprint(int projectId, Sprint sprint)
        {
            if(sprint.State != SprintState.ACTIVE)
            {
                throw new CheckedException($"Sprint has not been started.");

            }
            var date = DateTime.Today;

            _projectDAO.EndSprint(sprint.Id, date);
            foreach(var userStory in sprint.UserStories)
            {
                bool isFinished = true;
                foreach(var task in userStory.Tasks)
                {
                    if(!task.IsApproved)
                    {
                        isFinished = false;
                        _projectDAO.ResetAssignedMember(task.Id);
                    }
                }
                _projectDAO.UpdateUserStoryState(userStory.Id, isFinished? UserStoryState.RESOLVED: UserStoryState.BACKLOG);
            }

            var newSprint = new Sprint()
            {
                Order = sprint.Order + 1,
                State = SprintState.QUEUING,
            };
            _projectDAO.AddSprint(projectId, newSprint);
            var project = _projectDAO.GetProjectById(projectId);
            return project.Sprints.OrderByDescending(s => s.Order).First();
        }

        public void ApproveTask(int taskId)
        {
            _projectDAO.ApproveTask(taskId);
        }

        public void DenyTask(int taskId)
        {
            _projectDAO.DenyTask(taskId);
        }

        public List<Project> GetAllUnapprovedProjects(int userId)
        {
            var projects = _projectDAO.GetAllProjectsByMember(userId).Where(p => p.CreatedUser.Id == userId);
            var selectedProjects = new List<Project>();

            foreach (var project in projects)
            {
                var member = project.Members.FirstOrDefault(m => m.User.Id == userId);
                bool projectHasTask = false;
                var epicsToRemove = new List<Epic>();
                foreach (var epic in project.Epics)
                {
                    bool epicHasTask = false;
                    var userStoriesToRemove = new List<UserStory>();
                    foreach (var userStory in epic.UserStories)
                    {
                        if (userStory.State != UserStoryState.IN_PROGRESS)
                        {
                            userStoriesToRemove.Add(userStory);

                            continue;
                        }
                        bool userStoryHasTask = false;
                        var tasksToRemove = new List<Task>();
                        foreach (var task in userStory.Tasks)
                        {
                            if ((task.IsDone) && !task.IsApproved)
                            {
                                userStoryHasTask = true;
                            }
                            else
                            {
                                tasksToRemove.Add(task);
                            }
                        }
                        foreach (var task in tasksToRemove)
                        {
                            userStory.Tasks.Remove(task);
                        }
                        if (userStoryHasTask)
                        {
                            epicHasTask = true;
                        }
                        else
                        {
                            userStoriesToRemove.Add(userStory);
                        }
                    }
                    foreach (var userStory in userStoriesToRemove)
                    {
                        epic.UserStories.Remove(userStory);
                    }
                    if (epicHasTask)
                    {
                        projectHasTask = true;
                    }
                    else
                    {
                        epicsToRemove.Add(epic);
                    }
                }
                foreach (var epic in epicsToRemove)
                {
                    project.Epics.Remove(epic);
                }
                if (projectHasTask)
                {
                    selectedProjects.Add(project);
                }
            }

            return selectedProjects;
        }

        public void UpdateTaskCompletion(int taskId)
        {
            var task = _projectDAO.GetTaskById(taskId);
            if(task.IsDone)
            {
                _projectDAO.UpdateTaskCompletion(taskId, false);
            }
            else
            {
                _projectDAO.UpdateTaskCompletion(taskId, true);
            }
        }

        public List<Project> GetAllUndoneProjects(int userId)
        {
            var projects = _projectDAO.GetAllProjectsByMember(userId);
            var selectedProjects = new List<Project>();
            
            foreach(var project in projects)
            {
                var member = project.Members.FirstOrDefault(m => m.User.Id == userId);
                bool projectHasTask = false;
                var epicsToRemove = new List<Epic>();
                foreach(var epic in project.Epics)
                {
                    bool epicHasTask = false;
                    var userStoriesToRemove = new List<UserStory>();
                    foreach(var userStory in epic.UserStories)
                    {
                        if(userStory.State != UserStoryState.IN_PROGRESS)
                        {
                            userStoriesToRemove.Add(userStory);

                            continue;
                        }
                        bool userStoryHasTask = false;
                        var tasksToRemove = new List<Task>();
                        foreach(var task in userStory.Tasks)
                        {
                            if(task.AssignedMember == null)
                            {
                                tasksToRemove.Add(task);
                                continue;
                            }
                            if ((!task.IsDone) && task.AssignedMember.Id == member.Id)
                            {
                                userStoryHasTask = true;
                            }
                            else
                            {
                                tasksToRemove.Add(task);
                            }
                        }
                        foreach (var task in tasksToRemove)
                        {
                            userStory.Tasks.Remove(task);
                        }
                        if (userStoryHasTask)
                        {
                            epicHasTask = true;
                        }
                        else
                        {
                            userStoriesToRemove.Add(userStory);
                        }
                    }
                    foreach(var userStory in userStoriesToRemove)
                    {
                        epic.UserStories.Remove(userStory);
                    }
                    if(epicHasTask)
                    {
                        projectHasTask = true;
                    }
                    else
                    {
                        epicsToRemove.Add(epic);
                    }
                }
                foreach(var epic in epicsToRemove)
                {
                    project.Epics.Remove(epic);
                }
                if(projectHasTask)
                {
                    selectedProjects.Add(project);
                }
            }

            return selectedProjects;
        }

        public void AssignToTask(int memberId, int taskId)
        {
            _projectDAO.AssignTask(memberId, taskId);
        }

        public void AddTask(int userStoryId, string taskTitle)
        {
            var userStory = _projectDAO.GetUserStoryById(userStoryId);
            if(userStory.State == UserStoryState.RESOLVED)
            {
                throw new CheckedException("Cannot add task to a resolved user story.");
            }
            var task = new Task()
            {
                Title = taskTitle,
                IsDone = false,
                IsApproved = false
            };
            _projectDAO.AddTask(userStoryId, task);
        }

        public void ChangeMemberRole(int changingMemberId, Role selectedRole)
        {
            _projectDAO.UpdateMemberRole(changingMemberId, selectedRole);
        }

        public void EditProject(int id, string projectName, string projectKey)
        {
            if (!Regex.IsMatch(projectKey, "\\b\\w{2,5}\\b"))
            {
                throw new CheckedException("Key must contains only uppercase alphabetical letters or digits, and must have at least 2 and at most 5 characters.");
            }

            if (_projectDAO.GetProjectByKey(projectKey).Id != id)
            {
                throw new CheckedException($"Key {projectKey} has already been used.");
            }

            _projectDAO.EditProjectInfo(id, projectName, projectKey);
        }

        public Project GetProjectById(int projectId)
        {
            return _projectDAO.GetProjectById(projectId);
        }

        public void RemoveMemberFromProject(int projectId, int memberId)
        {
            var project = _projectDAO.GetProjectById(projectId);
            var member = _projectDAO.GetMemberById(memberId);
            if(member.User.Id == project.CreatedUser.Id)
            {
                throw new CheckedException("Cannot remove project's creator");
            }
            _projectDAO.DeleteMember(projectId, memberId);
        }

        public IEnumerable<Project> GetAllProjectsByMember(int id)
        {
            return _projectDAO.GetAllProjectsByMember(id);
        }
        public List<User> GetAllUsersNotInProject(int projectId)
        {
            return _projectDAO.GetAllUsersNotInProject(projectId);
        }

        public Member AddUserToProject(Project project, User user, Role role)
        {
            var member = new Member()
            {
                Role = role,
                User = user,
                Project = project
            };

            _projectDAO.InsertMember(project.Id, member);

            return _projectDAO.GetMemberByUserId(project.Id, user.Id);
        }
    }
}
