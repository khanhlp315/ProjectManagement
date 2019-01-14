using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserStory
    {
        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int StoryPoints
        {
            get;
            set;
        }
         
        public UserStoryState State
        {
            get;
            set;
        }

        public ObservableCollection<Task> Tasks
        {
            get;
            set;
        }

        public ObservableCollection<Sprint> Sprints
        {
            get;
            set;
        }
    }
}
