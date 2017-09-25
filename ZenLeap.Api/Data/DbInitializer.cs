using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ZenLeap.Api.Models;
using ZenLeap.Api.Authorization;

namespace ZenLeap.Api.Data
{
    public class DbInitializer : IDbInitializer
    {
		private readonly DataContext _context;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;


		public DbInitializer(
			DataContext context,
			UserManager<User> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		//This example just creates an Administrator role and one Admin users
		public void Initialize()
		{
			//create database schema if none exists
			_context.Database.EnsureCreated();

			//If there is already an Administrator role, abort
			if (_context.Roles.AnyAsync(r => r.Name == "Administrator").Result) return;

            //Create the Administartor Role
            // _roleManager.CreateAsync(new ApplicationRole("Administrator"));
           var adminRole = new IdentityRole 
            {
                Name = "admin",
                NormalizedName = "Administrator"
            };

            _roleManager.CreateAsync(adminRole);

			//Create the default Admin account and apply the Administrator role
			//From: https://gist.github.com/mombrea/9a49716841254ab1d2dabd49144ec092
			//string user = "me@myemail.com";
			//string password = "z0mgchangethis";
			//await _userManager.CreateAsync(new ApplicationUser { UserName = user, Email = user, EmailConfirmed = true }, password);
			//await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Administrator");


			// TODO: Add Users to roles: https://github.com/aspnet/Docs/blob/master/aspnetcore/security/authorization/secure-data/samples/final/Data/SeedData.cs
			//if (_context.Users.AnyAsync().Result)
			//{
			//	return; // DB has been seeded
			//}

			SeedDB(_context, _userManager, _roleManager);


		}

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

            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(newUser, testUserPw);
                await userManager.AddToRoleAsync(newUser, Constants.GlobalAdministratorsRole);
            }

            return user.Id.ToString(); 
        }

        private static async Task<IdentityResult> EnsureRole(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, User newUser, string role)
        {
            IdentityResult IR = null;
            //var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
				var newRole = new IdentityRole
				{
					Name = role,
					NormalizedName = role
				};
                IR = await roleManager.CreateAsync(newRole);
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
            /* 
             * Ensure base roles exist
             * Global Admin
             * Team Roles
             * Event Roles
            */

            // Create Global Admin
			var globalAdmin = new User
			{
				FirstName = "Joe",
				LastName = "Shepherd",
				Email = "joe@eurussolutions.com",
                UserName = "joe@eurussolutions.com",
				//AssignedTasks = null,
				//Projects = null
			};

            var id = await EnsureUser(userManager, globalAdmin, "Pass@word1");
            await EnsureRole(roleManager, userManager, globalAdmin, Constants.GlobalAdministratorsRole);

            var users = new User[]
            {
                new User{
                    FirstName="Carson",
                    LastName="Alexander", 
                    Email="test@test.com", 
                    //AssignedTasks=null, 
                    //Projects=null
                },
                new User { 
                    FirstName = "Edward", 
                    LastName = "Norton", 
                    Email = "test@test.com", 
                    //AssignedTasks = null, 
                    //Projects = null
                },
                new User{
                    FirstName="Ginger",
                    LastName="Martin", 
                    Email="test@test.com", 
                    //AssignedTasks=null, 
                    //Projects=null
                }
            };

            foreach (User u in users)
            {
                await EnsureUser(userManager, u, "Pass@word1");
            }
            context.SaveChanges();



            var companies = new Company[]
            {
                new Company{CompanyName="EURUS", OwnerId=id, DateEstablished=DateTime.Parse("01/07/2017"), Projects=null},
                new Company{CompanyName="ZenLeap", OwnerId=id, DateEstablished=DateTime.Parse("01/07/2017"), Projects=null}
            };
            //foreach (Company c in companies)
            //{
            //    context.Companies.Add(c);
            //}
            context.SaveChanges();



            var projects = new Project[]
            {
                new Project{Title="ZenLeap Product Launch", CompanyId=2, Description="Project to manage the launch of the ZenLeap Proejct", ProjectOwnerId=id}
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