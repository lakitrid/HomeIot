using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Fr.Lakitrid.CentralBrain;

namespace CentralBrain.Migrations
{
    [DbContext(typeof(BrainDbContext))]
    [Migration("20161106144731_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
