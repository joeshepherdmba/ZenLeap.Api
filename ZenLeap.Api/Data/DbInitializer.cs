using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Data
{
    public static class DbInitializer
    {
        public async static Task Initialize(DataContext context)
        {

            if (await context.Users.AnyAsync())
            {
                return; // DB has been seeded
            }

            var users = new User[]
            {
				new User{FirstName="Joe",LastName="Shepherd", Email="joe@eurussolutions", AssignedTasks=null, Projects=null, Password="Pass@word1"},
                new User{FirstName="Carson",LastName="Alexander", Email="test@test.com", AssignedTasks=null, Projects=null, Password="Pass@word1"},
                new User { FirstName = "Edward", LastName = "Norton", Email = "test@test.com", AssignedTasks = null, Projects = null, Password = "Pass@word1"},
                new User{FirstName="Ginger",LastName="Martin", Email="test@test.com", AssignedTasks=null, Projects=null, Password="Pass@word1"}
            };
            foreach(User u in users){
                context.Users.Add(u);
            }
            context.SaveChanges();

            var companies = new Company[]
            {
                new Company{CompanyName="EURUS", OwnerId=1, DateEstablished=DateTime.Parse("01/07/2017"), Projects=null},
				new Company{CompanyName="ZenLeap", OwnerId=2, DateEstablished=DateTime.Parse("01/07/2017"), Projects=null}
            };
			foreach (Company c in companies)
			{
                context.Companies.Add(c);
			}
			context.SaveChanges();

            var projects = new Project[]
            {
                new Project{Title="ZenLeap Product Launch", CompanyId=2, Description="Project to manage teh launch of the ZenLeap Proejct", ProjectOwnerId=1}
            };
			foreach (Project p in projects)
			{
				context.Projects.Add(p);
			}
			context.SaveChanges();
        }
    }
}

