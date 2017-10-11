using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Data
{
    public class DataContext : IdentityDbContext//<User, ApplicationRole, int>//DbContext
    {
        public DataContext()
            :base(){
			
        }
		public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
		}

        public DbSet<User> Users{ get; set; }
		public DbSet<IdentityRole> Roles { get; set; }
        //public DbSet<Company> Companies { get; set; }
		public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Activity> Events { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Specify the path of the database here
			optionsBuilder.UseSqlite("Filename=./ZenLeap_Launch.sqlite");
		}


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			// TODO: http://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
			//builder.Entity<TeamOwners>()
            //       .HasKey(x => new { x.TeamId, x.OwnerId })
            //       .HasName("TeamOwnersId");

            //builder.Entity<TeamOwners>()
                   //.HasOne(to => to.Owner)
                   //.WithMany(u => u.OwnedTeams)
                   //.HasForeignKey(to => to.OwnerId);


            builder.Entity<UserTeams>().HasKey(x => new { x.TeamId, x.MemberId }).HasName("TeamMembersId");

			builder.Entity<UserTeams>()
				   .HasOne(tm => tm.Team)
				   .WithMany(t => t.Members)
				   .HasForeignKey(tm => tm.TeamId);

			builder.Entity<UserTeams>()
				   .HasOne(tm => tm.Member)
				   .WithMany(m => m.Teams)
				   .HasForeignKey(tm => tm.MemberId);

			/*
             * Creating:

                1. Create new AB

                2. Set AB.A = New A

                3. Set AB.B = New B

                4. Db.SaveChanges()



                Deleting:



                1. Find and delete AB

                2. Db.SaveChanges()



                (Deletes are cascaded by default, so deletion is a bit easier than creation.)

            */

		}
    }
}
