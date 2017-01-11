using HBlog.Web.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace HBlog.Web
{
	public class Startup
    {
		private IConfiguration config;
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile(@"configs/app.json")
				.AddJsonFile($"configs/app.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			config = builder.Build();
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
			services.AddSingleton<IConfiguration>(config);
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Administrator", policy => policy.RequireUserName("hance"));

				options.AddPolicy("AgeOver25", policy => policy.Requirements.Add(new AgeAuthorizationRequirement(25)));
			});

			services.AddSingleton<IAuthorizationHandler, AgeAuthorizationHandler>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				AuthenticationScheme = "MyCookieMiddlewareInstance",
				LoginPath = new PathString("/Account/Login/"),
				//未授权
				AccessDeniedPath = new PathString("/Account/Forbidden/"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true,

				CookieHttpOnly=true,
				ExpireTimeSpan=DateTime.Now.AddDays(1).TimeOfDay
			});

			app.UseMvcWithDefaultRoute();
			app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}
