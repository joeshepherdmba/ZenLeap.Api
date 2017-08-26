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
        

        public async static Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context)
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

            SeedDB(context, userManager, roleManager);

        }

        #region snippet_CreateRoles        

        private static async Task<string> EnsureUser(UserManager<User> userManager, User newUser, string testUserPw)
        {
            //var userManager = serviceProvider.GetRequiredService<UserManager<User>>();


            var user = await userManager.FindByNameAsync(newUser.UserName);
            if (user == null)
            {
                await userManager.CreateAsync(user, testUserPw);
                await userManager.AddToRoleAsync(user, Constants.GlobalAdministratorsRole);
            }

            return user.Id.ToString(); //TODO: Change Model to string from Int
        }

        private static async Task<IdentityResult> EnsureRole(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, User newUser, string role)
        {
            IdentityResult IR = null;
            //var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            //var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(newUser.Id.ToString());

            IR = await userManager.AddToRoleAsync(user, role); 

            return IR;
        }
        #endregion

        #region "seedDB"
        public static async void SeedDB(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create Global Admin
			var globalAdmin = new User
			{
				FirstName = "Joe",
				LastName = "Shepherd",
				Email = "joe@eurussolutions",
				AssignedTasks = null,
				Projects = null
			};

            var id = await EnsureUser(userManager, globalAdmin, "Pass@word1");
            await EnsureRole(roleManager, userManager, globalAdmin, Constants.GlobalAdministratorsRole);

            var users = new User[]
            {
                new User{
                    FirstName="Carson",
                    LastName="Alexander", 
                    Email="test@test.com", 
                    AssignedTasks=null, 
                    Projects=null
                },
                new User { 
                    FirstName = "Edward", 
                    LastName = "Norton", 
                    Email = "test@test.com", 
                    AssignedTasks = null, 
                    Projects = null
                },
                new User{
                    FirstName="Ginger",
                    LastName="Martin", 
                    Email="test@test.com", 
                    AssignedTasks=null, 
                    Projects=null
                }
            };

            foreach (User u in users)
            {
                await EnsureUser(userManager, u, "Pass@word1");
            }
            //context.SaveChanges();



            var companies = new Company[]
            {
                new Company{CompanyName="EURUS", OwnerId=id, DateEstablished=DateTime.Parse("01/07/2017"), Projects=null},
                new Company{CompanyName="ZenLeap", OwnerId=id, DateEstablished=DateTime.Parse("01/07/2017"), Projects=null}
            };
            foreach (Company c in companies)
            {
                context.Companies.Add(c);
            }
            context.SaveChanges();



            var projects = new Project[]
            {
                new Project{Title="ZenLeap Product Launch", CompanyId=2, Description="Project to manage teh launch of the ZenLeap Proejct", ProjectOwnerId=id}
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