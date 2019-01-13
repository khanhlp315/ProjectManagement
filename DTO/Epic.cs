using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Epic
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

        public ObservableCollection<UserStory> UserStories
        {
            get;
            set;
        }

        public Project Project
        {
            get;
            set; 
        }
    }
}
