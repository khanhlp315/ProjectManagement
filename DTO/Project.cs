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
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }
        
        public string Key
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? StartDate
        {
            get;
            set;
        }

        public User CreatedUser
        {
            get;
            set;
        }

        public ObservableCollection<Member> Members
        {
            get;
            set;
        }

        public ObservableCollection<Epic> Epics
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
