using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DTO
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }
        
        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public Permission Permission
        {
            get;
            set;
        }
    }
}
