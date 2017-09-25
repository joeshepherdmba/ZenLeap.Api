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
using Microsoft.Extensions.Logging;

namespace ZenLeap.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment())
			{
				// For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
				builder.AddUserSecrets<Startup>();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

        public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
            services.AddMvc();

            services.AddDbContext<DataContext>(options =>
               options.UseSqlite("Filename=./ZenLeap_Launch.sqlite"));

			// Identity 2.0
			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<DataContext>()
				.AddDefaultTokenProviders();

			// Add Database Initializer
			services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddAuthorization();

			 //Configure Identity
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

			  //Add application services.
			  //services.AddTransient<IEmailSender, AuthMessageSender>();
			  //services.AddTransient<ISmsSender, AuthMessageSender>();
			

			// Add application services.
			//services.AddTransient<IEmailSender, AuthMessageSender>();
			//services.AddTransient<ISmsSender, AuthMessageSender>();
		}


		// This method gets called by the runtime. Use this method to add services to the container.
		//      public void ConfigureServices(IServiceCollection services)
		//      {
		//	services.AddMvc();

		//          services.AddDbContext<DataContext>(options =>
		//		options.UseSqlite("Filename=./ZenLeap_Launch.sqlite"));

		//          // Add Database Initializer
		//          //services.AddScoped<IDbInitializer, DbInitializer>();
		//          services.AddTransient<IDbInitializer, DbInitializer>();


		//          // Identity 2.0
		//          services.AddIdentity<User, ApplicationRole>()
		//              .AddEntityFrameworkStores<DataContext>()
		//              .AddDefaultTokenProviders();

		//          services.AddAuthorization();

		//	// If you want to tweak Identity cookies, they're no longer part of IdentityOptions.
		//	services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");

		//          // Facebook Authentication
		//          //services.AddAuthentication()
		//	//		.AddFacebook(options => {
		//	//			options.AppId = Configuration["auth:facebook:appid"];
		//	//			options.AppSecret = Configuration["auth:facebook:appsecret"];
		//	//		});

		//	//// Google Authentication
		//	//services.AddAuthentication()
		// //         		.AddGoogle(options => {
		// //         			options.ClientId = Configuration["auth:google:clientid"];
		// //         			options.ClientSecret = Configuration["auth:google:clientsecret"];
		// //         		});

		//	//// Microsoft Authentication
		//	//services.AddAuthentication()
		//          		//.AddMicrosoftAccount(options => {
		//          		//	options.ClientId = Configuration["auth:microsoft:clientid"];
		//          		//	options.ClientSecret = Configuration["auth:microsoft:clientsecret"];
		//          		//});

		//	// Configure Identity
		//	services.Configure<IdentityOptions>(options =>
		//	{
		//		// Password settings
		//		options.Password.RequireDigit = true;
		//		options.Password.RequiredLength = 8;
		//		options.Password.RequireNonAlphanumeric = false;
		//		options.Password.RequireUppercase = true;
		//		options.Password.RequireLowercase = false;

		//		// Lockout settings
		//		options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
		//		options.Lockout.MaxFailedAccessAttempts = 10;

		//		// User settings
		//		options.User.RequireUniqueEmail = true;
		//	});

		//	services.AddMvc()
		//		.AddRazorPagesOptions(options =>
		//		{
		//			options.Conventions.AuthorizeFolder("/Account/Manage");
		//			options.Conventions.AuthorizePage("/Account/Logout");
		//		});

		//	// Register no-op EmailSender used by account confirmation and password reset during development
		//	// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
		//	services.AddSingleton<IEmailSender, EmailSender>();

		//	// Add application services.
		//	//services.AddTransient<IEmailSender, AuthMessageSender>();
		//	//services.AddTransient<ISmsSender, AuthMessageSender>();
		//}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDbInitializer dbInitializer)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();

				// Browser Link is not compatible with Kestrel 1.1.0
				// For details on enabling Browser Link, see https://go.microsoft.com/fwlink/?linkid=840936
				// app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			//Generate EF Core Seed Data
			//var userManager = app.ApplicationServices.GetService<UserManager<User>>();
			//var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();
			//var context = app.ApplicationServices.GetService<DataContext>();
//			//dbInitializer.Initialize();
			
            //DbInitializer.Initialize(userManager, roleManager, context).Wait();

			// Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		//     public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer DbInitializer)
		//     {
		//         if (env.IsDevelopment())
		//         {
		//             app.UseDeveloperExceptionPage();
		//         }
		//         else
		//         {
		//             app.UseExceptionHandler("/Error");
		//         }

		//app.UseAuthentication();
		//         app.UseStaticFiles();
		////app.UseIdentity();

		////var context = new DataContext();
		////DbInitializer.Initialize(context);
		////var userManager = app.ApplicationServices.GetService<UserManager<User>>();
		////var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

		////DbInitializer.Initialize(userManager, roleManager, context).Wait();

		////Generate EF Core Seed Data

		//DbInitializer.Initialize();

		//app.UseMvc(routes =>
		//        {
		//routes.MapRoute(
		//name: "api",
		//            template: "api/{controller}/{action=Index}/{id?}");
		//        routes.MapRoute(
		//            name: "default",
		//            template: "{controller}/{action=Index}/{id?}");
		//    });

		//}
	}
}
