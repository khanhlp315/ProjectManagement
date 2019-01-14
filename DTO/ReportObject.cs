using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ReportObject
    {
        public string ProjectKey
        {
            get;
            set;
        }
        public string ProjectName
        {
            get;
            set;
        }
        public string OwnerUsername
        {
            get;
            set;
        }
        public int SumOfMembers
        {
            get;
            set;
        }
        public int CurrentSprint
        {
            get;
            set;
        }
        public int EpicsRemained
        {
            get;
            set;
        }
        public int UserStoriesRemained
        {
            get;
            set;
        }
        public int StoryPointsAccumulated
        {
            get;
            set;
        }
    }
}
