using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using ZenLeap.Api.Data;
using ZenLeap.Api.Models;
using ZenLeap.Api.Services;

namespace ZenLeap.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();

            services.AddDbContext<DataContext>(options =>
				options.UseSqlite("Filename=./ZenLeap_Launch.sqlite"));
            
           
            /*
            // requires: using Microsoft.AspNetCore.Authorization;
			//           using Microsoft.AspNetCore.Mvc.Authorization;
			services.AddMvc(config =>
			{
				var policy = new AuthorizationPolicyBuilder()
								 .RequireAuthenticatedUser()
								 .Build();
				config.Filters.Add(new AuthorizeFilter(policy));
			});
            */

            // Identity 2.0
            services.AddIdentity<User, ApplicationRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization();

			// If you want to tweak Identity cookies, they're no longer part of IdentityOptions.
			services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");
			
            // Facebook Authentication
            //services.AddAuthentication()
			//		.AddFacebook(options => {
			//			options.AppId = Configuration["auth:facebook:appid"];
			//			options.AppSecret = Configuration["auth:facebook:appsecret"];
			//		});

			//// Google Authentication
			//services.AddAuthentication()
   //         		.AddGoogle(options => {
   //         			options.ClientId = Configuration["auth:google:clientid"];
   //         			options.ClientSecret = Configuration["auth:google:clientsecret"];
   //         		});

			//// Microsoft Authentication
			//services.AddAuthentication()
            		//.AddMicrosoftAccount(options => {
            		//	options.ClientId = Configuration["auth:microsoft:clientid"];
            		//	options.ClientSecret = Configuration["auth:microsoft:clientsecret"];
            		//});

			// Configure Identity
			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;

				// Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				options.Lockout.MaxFailedAccessAttempts = 10;

				// User settings
				options.User.RequireUniqueEmail = true;
			});

			services.AddMvc()
				.AddRazorPagesOptions(options =>
				{
					options.Conventions.AuthorizeFolder("/Account/Manage");
					options.Conventions.AuthorizePage("/Account/Logout");
				});

			// Register no-op EmailSender used by account confirmation and password reset during development
			// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
			services.AddSingleton<IEmailSender, EmailSender>();

			// Add application services.
			//services.AddTransient<IEmailSender, AuthMessageSender>();
			//services.AddTransient<ISmsSender, AuthMessageSender>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

			app.UseAuthentication();
            app.UseStaticFiles();
            //app.UseIdentity();

   //         DataContext context = new DataContext();
			//// DbInitializer.Initialize(context);
			//var userManager = app.ApplicationServices.GetService<UserManager<User>>();
			//var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

            //DbInitializer.Initialize(userManager, roleManager, context).Wait();

			app.UseMvc(routes =>
            {
				routes.MapRoute(
					name: "api",
                    template: "api/{controller}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}
