using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskDatesCommon.Models;

namespace TestTaskDatesWepApplication.Database
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DateRange> DateRanges { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            int newDateRangeID = 1;
            modelBuilder.Entity<DateRange>().HasData(
                new DateRange { ID = newDateRangeID++, Start = DateTime.Parse("2018-01-01").Date, End = DateTime.Parse("2018-01-03").Date },
                new DateRange { ID = newDateRangeID++, Start = DateTime.Parse("2018-01-01").Date, End = DateTime.Parse("2018-01-31").Date },
                new DateRange { ID = newDateRangeID++, Start = DateTime.Parse("2018-01-03").Date, End = DateTime.Parse("2018-01-05").Date }
                );

            modelBuilder.Entity<User>().HasData(
                new User { Login = "user", Password = "123", Role = "User" }
                );
        }
    }
}
