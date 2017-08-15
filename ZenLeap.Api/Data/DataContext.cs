using System;
using Microsoft.EntityFrameworkCore;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            
        }

        public DbSet<User> Users{ get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Specify the path of the database here
			optionsBuilder.UseSqlite("Filename=./ZenLeap_Launch.sqlite");
		}
    }
}
