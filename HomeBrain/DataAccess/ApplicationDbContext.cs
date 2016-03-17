using Common.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain.DataAccess
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("ApplicationDb")
        {
        }

        public DbSet<TaskInfo> Tasks { get; set; }

        public DbSet<Command> Commands { get; set; }

        public DbSet<TimeSerie> TimeSeries { get; set; }

        public DbSet<TimeSerie> DayTimeSeries { get; set; }
    }
}
