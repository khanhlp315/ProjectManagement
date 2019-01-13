﻿using System;
using System.Collections.Generic;
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

        public Epic Epic
        {
            get;
            set;
        }
    }
}
