using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Common.Domain;

namespace WebSite.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TaskInfo> Tasks { get; set; }

        public DbSet<Command> Commands { get; set; }

        public DbSet<TimeSerie> TimeSeries { get; set; }

        public DbSet<TimeSerie> DayTimeSeries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
