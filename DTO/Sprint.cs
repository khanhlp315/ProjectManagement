using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Sprint
    {
        public int Id
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        [Column(TypeName ="date")]
        public DateTime StartDate
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime EndDate
        {
            get;
            set;
        }

        public SprintState State
        {
            get;
            set;
        }

        public ObservableCollection<UserStory> UserStories
        {
            get;
            set;
        }
    }
}
