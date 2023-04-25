using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stud.Model.Models
{
    public class course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public double CoursePrice { get; set; }
       
    }
}
