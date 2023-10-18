using Microsoft.EntityFrameworkCore;
using Sibers.Entities;
using Sibers.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.Domain
{
    public class SibersContext : DbContext
    {
        public DbSet<Company> Company { get; set; } = null!;
        public DbSet<Project> Project { get; set; } = null!;
        public DbSet<Employee> Employee { get; set; } = null!;
        public DbSet<Goal> Goal { get; set; } = null!;

        public SibersContext(DbContextOptions<SibersContext> options) : base(options) { }
    }
}
