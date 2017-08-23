using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZenLeap.Api.Data;
using ZenLeap.Api.Models;

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

            // Identoty 2.0
			services.AddIdentity<User, IdentityRole>()
        		.AddEntityFrameworkStores<DataContext>()
        		.AddDefaultTokenProviders();


			// If you want to tweak Identity cookies, they're no longer part of IdentityOptions.
			services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");
			
            // Facebook Authentication
            services.AddAuthentication()
					.AddFacebook(options => {
						options.AppId = Configuration["auth:facebook:appid"];
						options.AppSecret = Configuration["auth:facebook:appsecret"];
					});

			// Google Authentication
			services.AddAuthentication()
            		.AddGoogle(options => {
            			options.ClientId = Configuration["auth:google:clientid"];
            			options.ClientSecret = Configuration["auth:google:clientsecret"];
            		});

			// Microsoft Authentication
			services.AddAuthentication()
            		.AddMicrosoftAccount(options => {
            			options.ClientId = Configuration["auth:microsoft:clientid"];
            			options.ClientSecret = Configuration["auth:microsoft:clientsecret"];
            		});

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

            app.UseStaticFiles();

            // DataContext context = new DataContext();
            // DbInitializer.Initialize(context);

            app.UseAuthentication();

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
