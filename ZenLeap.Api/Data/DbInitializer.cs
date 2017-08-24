using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ZenLeap.Api.Models;
using ZenLeap.Api.Authorization;

namespace ZenLeap.Api.Data
{
    public static class DbInitializer
    {
        public async static Task Initialize(IServiceProvider serviceProvider, DataContext context)
        {
            // TODO: Add Users to roles: https://github.com/aspnet/Docs/blob/master/aspnetcore/security/authorization/secure-data/samples/final/Data/SeedData.cs
            if (await context.Users.AnyAsync())
            {
                return; // DB has been seeded
            }

            //var adminID = await EnsureUser(serviceProvider, "Pass@word1", "admin@contoso.com");
            ////await EnsureRole(serviceProvider, adminID, Constants.ContactAdministratorsRole);

            //// allowed user can create and edit contacts that they create
            //var uid = await EnsureUser(serviceProvider, "Pass@word1", "manager@contoso.com");
            //await EnsureRole(serviceProvider, uid, Constants.CompanyManagersRole);

            SeedDB(context, serviceProvider);

        }

        #region snippet_CreateRoles        

        private static async Task<int> EnsureUser(IServiceProvider serviceProvider, User newUser, string testUserPw)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(newUser.UserName);
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Joe",
                    LastName = "Shepherd",
                    Email = "joe@eurussolutions",
                    AssignedTasks = null,
                    Projects = null
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, Constants.CompanyAdministratorsRole); // TODO: Create and change to GlobalAdminRole

            return IR;
        }
        #endregion

        #region "seedDB"
        public static async void SeedDB(DataContext context, IServiceProvider serviceProvider)
        {
            var users = new User[]
            {
                new User{FirstName="Carson",LastName="Alexander", Email="test@test.com", AssignedTasks=null, Projects=null, Password="Pass@word1"},
                new User { FirstName = "Edward", LastName = "Norton", Email = "test@test.com", AssignedTasks = null, Projects = null, Password = "Pass@word1"},
                new User{FirstName="Ginger",LastName="Martin", Email="test@test.com", AssignedTasks=null, Projects=null, Password="Pass@word1"}
            };

            foreach (User u in users)
            {
                await EnsureUser(serviceProvider, u, "Pass@word1");
            }
            //context.SaveChanges();



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
    #endregion
}