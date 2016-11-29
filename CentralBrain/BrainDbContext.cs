using Fr.Lakitrid.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fr.Lakitrid.CentralBrain
{
    public class BrainDbContext : DbContext
    {
        public BrainDbContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=192.168.1.21;Database=homedata;Username=homedata;Password=homedata");
        }

        public DbSet<PowerDay> PowerDay { get; set; }
    }
}
