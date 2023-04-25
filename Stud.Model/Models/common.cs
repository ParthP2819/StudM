using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stud.Model.Models
{
    public class common
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
