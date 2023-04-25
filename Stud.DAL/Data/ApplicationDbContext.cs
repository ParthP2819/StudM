using Microsoft.EntityFrameworkCore;
using Stud.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stud.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<student> student { get; set; }
        public DbSet<course> course { get; set; }
        public DbSet<admin> admin { get; set; }
    }
}
