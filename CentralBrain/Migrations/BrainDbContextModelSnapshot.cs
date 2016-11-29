using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Fr.Lakitrid.CentralBrain;

namespace CentralBrain.Migrations
{
    [DbContext(typeof(BrainDbContext))]
    partial class BrainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("Fr.Lakitrid.Common.Models.PowerDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("LowHourIndex");

                    b.Property<decimal>("PeekHourIndex");

                    b.HasKey("Id");

                    b.ToTable("PowerDay");
                });
        }
    }
}
