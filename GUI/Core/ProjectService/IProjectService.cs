using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Core.ProjectService
{
    public interface IProjectService
    {
        Project CreateProject(int userId, string key, string name);
        IEnumerable<Project> GetAllProjectsByMember(int id);
        List<User> GetAllUsersNotInProject(int projectId);
        Member AddUserToProject(User user, Project project, Role role);
        void EditProject(int id, string projectName, string projectKey);
        void RemoveMemberFromProject(int projectId, int memberId);
        void ChangeMemberRole(int changingMemberId, Role selectedRole);
        Project GetProjectById(int id);
        Epic AddEpic(int projectId, string epicTitle, string epicDescription);
        void AddUserStory(int epicId, string title, string description, int storyPoints);
        Sprint GetNextSprint(int projectId);
        void AddUserStoryToSprint(int sprintId, UserStory userStory);
        void StartSprint(Sprint sprint);
        Sprint EndSprint(int projectId, Sprint sprint);
    }
}
