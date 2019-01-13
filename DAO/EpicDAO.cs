using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DTO;

namespace DAO
{
    public class EpicDAO
    {
        public bool CheckTitle(int projectId, string epicTitle)
        {
            using (var context = new ProjectManagementContext())
            {
                var same = context.Epics.Include(epic => epic.Project).FirstOrDefault(epic => epic.Title == epicTitle && epic.Project.Id == projectId);
                return same != null;
            }
        }

        public Epic GetEpicByTitle(int projectId, string title)
        {
            using (var context = new ProjectManagementContext())
            {
                return context.Epics.Include(epic => epic.Project).FirstOrDefault(epic => epic.Title == title && epic.Project.Id == projectId);
            }
        }

        public bool CheckUserStory(int epicId, string title)
        {
            using (var context = new ProjectManagementContext())
            {
                var selectedEpic = context.Epics.Include(epic => epic.UserStories).FirstOrDefault(epic => epic.Id == epicId);
                if(selectedEpic == null)
                {
                    return false;
                }
                var userStoryWithTitle = selectedEpic.UserStories.FirstOrDefault(userStory => userStory.Title == title);
                return userStoryWithTitle != null;
            }
        }

        public void AddUserStory(int epicId, string title, string description, int storyPoints)
        {
            using (var context = new ProjectManagementContext())
            {
                var selectedEpic = context.Epics.Include(epic => epic.UserStories).FirstOrDefault(epic => epic.Id == epicId);
                selectedEpic.UserStories.Add(new UserStory()
                {
                    Title = title,
                    Description = description,
                    StoryPoints = storyPoints,
                    State = UserStoryState.BACKLOG
                });
                context.SaveChanges();
            }
        }
    }
}
