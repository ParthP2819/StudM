using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stud.Model.Models
{
    public class admin
    {
        [Key]
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string AdminPassword { get; set; }
        public string AdminEmail { get; set; }

    }
}
