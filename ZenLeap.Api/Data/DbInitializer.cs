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

        private static async Task<string> CreateUser(UserManager<User> userManager, User newUser, string testUserPw, string role=null)
        {

            var user = await userManager.FindByEmailAsync(newUser.Email);

            if (user == null)
            {
                var result = await userManager.CreateAsync(newUser, testUserPw);

                if(result.Succeeded) {

					if (role != null)
					{
						await userManager.AddToRoleAsync(newUser, role);
					}
                    return newUser.Id;
				}			
            }

            return user.Id;
        }

        private static async Task<IdentityResult> AddUserToRole(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, User newUser, IdentityRole role)
        {
            IdentityResult IR = null;

            // If role does not exist, create it
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
				await CreateRole(roleManager, role);
            }

            var user = await userManager.FindByIdAsync(newUser.Id.ToString());

            IR = await userManager.AddToRoleAsync(user, role.Name); 

            return IR;
        }

        private static async Task<IdentityResult> CreateRole(RoleManager<IdentityRole> roleManager, IdentityRole role) {
			IdentityResult IR = null;

			if (!await roleManager.RoleExistsAsync(role.Name))
			{
				var newRole = new IdentityRole
				{
					Name = role.Name,
					NormalizedName = role.NormalizedName
				};
				IR = await roleManager.CreateAsync(newRole);
			}
            return IR;
        }

        #endregion

        #region "seedDB"
        public static async void SeedDB(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Set up Roles
            var newRoles = new IdentityRole[]
            {
                new IdentityRole {
                    Name = Constants.GlobalAdministratorsRole,
                    NormalizedName = Constants.GlobalAdministratorsRoleNormalizedName
                },
				new IdentityRole {
					Name = Constants.TeamOwnersRole,
					NormalizedName = Constants.TeamOwnersRoleNormalizedName
				},
				new IdentityRole {
					Name = Constants.TeamAdministratorsRole,
					NormalizedName = Constants.TeamAdministratorsRoleNormalizedName
				},
				new IdentityRole {
					Name = Constants.TeamMembersRole,
					NormalizedName = Constants.TeamMembersRoleNormalizedName
				},
				new IdentityRole {
					Name = Constants.ActivityAdministratorsRole,
					NormalizedName = Constants.ActivityAdministratorsRoleNormalizedName
				},
				new IdentityRole {
                    Name = Constants.ActivityMembersRole,
                    NormalizedName = Constants.ActivityMembersRoleNormalizedName
                }
			};

            // Create Roles
            foreach (IdentityRole r in newRoles) {
                await CreateRole(roleManager, r);
            }

            // Create Global Admin User
			var globalAdmin = new User
			{
				FirstName = "Joe",
				LastName = "Shepherd",
				Email = "joe@eurussolutions.com",
                UserName = "joe@eurussolutions.com"
			};

            var id = await CreateUser(userManager, globalAdmin, "Pass@word1", Constants.GlobalAdministratorsRole);

            var users = new User[]
            {
                new User{
                    FirstName="Carson",
                    LastName="Alexander", 
                    Email="test1@test.com", 
                    UserName = "test1@test.com"
                },
                new User { 
                    FirstName = "Edward", 
                    LastName = "Norton",
					Email = "test2@test.com",
					UserName = "test2@test.com"
                },
                new User{
                    FirstName="Ginger",
                    LastName="Martin",
					Email="test3@test.com",
					UserName = "test3@test.com"
                }
            };

            // Set up array to hold new user ids
            var ids = new string[3];
            var i = 0;
            foreach (User u in users)
            {
                var uid = await CreateUser(userManager, u, "Pass@word1", Constants.TeamMembersRole);
                ids[i] = uid;
                i++;
            }
            context.SaveChanges();

            // Create new Teams
            var teams = new Team[]
            {
                new Team{ TeamName="EURUS", OwnerId=ids[0], DateEstablished=DateTime.Parse("01/07/2017") },
                new Team{ TeamName="ZenLeap", OwnerId=ids[1], DateEstablished=DateTime.Parse("01/07/2017") },
                new Team{ TeamName="Swarm", OwnerId=ids[2], DateEstablished=DateTime.Parse("01/07/2017") }
            };
            foreach (Team t in teams)
            {
                context.Teams.Add(t);
            }
            context.SaveChanges();
        }
    }
    #endregion
}