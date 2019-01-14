using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUS;
using BUS.Exceptions;
using DTO;

namespace GUI.Core.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly ProjectBUS _projectBUS = new ProjectBUS();

        public Epic AddEpic(int projectId, string epicTitle, string epicDescription)
        {
            return _projectBUS.AddEpic(projectId, epicTitle, epicDescription);
        }

        public void AddUserStory(int epicId, string title, string description, int storyPoints)
        {
            _projectBUS.AddUserStory(epicId, title, description, storyPoints);
        }

        public void AddUserStoryToSprint(int sprintId, UserStory userStory)
        {
            _projectBUS.AddUserStoryToSprint(sprintId, userStory);
        }

        public Member AddUserToProject(User user, Project project, Role role)
        {
            try
            {
                return _projectBUS.AddUserToProject(project, user, role);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public void ChangeMemberRole(int changingMemberId, Role selectedRole)
        {
            _projectBUS.ChangeMemberRole(changingMemberId, selectedRole);
        }

        public void EditProject(int id, string projectName, string projectKey)
        {
            try
            {
                _projectBUS.EditProject(id, projectName, projectKey);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public IEnumerable<Project> GetAllProjectsByMember(int id)
        {
            try
            {
                return _projectBUS.GetAllProjectsByMember(id);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public List<User> GetAllUsersNotInProject(int projectId)
        {
            try
            {
                return _projectBUS.GetAllUsersNotInProject(projectId);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public Sprint GetNextSprint(int projectId)
        {
            try
            {
                return _projectBUS.GetNextSprint(projectId);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public Project GetProjectById(int id)
        {
            try
            {
                return _projectBUS.GetProjectById(id);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public void RemoveMemberFromProject(int projectId, int memberId)
        {
            try
            {
                _projectBUS.RemoveMemberFromProject(projectId, memberId);
            }
            catch(CheckedException e)
            {
                throw e;
            }
        }

        public Sprint EndSprint(int projectId, Sprint sprint)
        {
            try
            {
                return _projectBUS.EndSprint(projectId, sprint);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        void IProjectService.CreateProject(int userId, string key, string name)
        {
            try
            {
                 _projectBUS.CreateProject(userId, key, name);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public void StartSprint(Sprint sprint)
        {
            try
            {
                _projectBUS.StartSprint(sprint);
            }
            catch (CheckedException e)
            {
                throw e;
            }
        }

        public void AddTask(int userStoryId, string taskTitle)
        {
            try
            {
                _projectBUS.AddTask(userStoryId, taskTitle);
            }
            catch(CheckedException e)
            {
                throw e;
            }
        }

        public void AssignToTask(int memberId, int taskId)
        {
            _projectBUS.AssignToTask(memberId, taskId);
        }
    }
}
