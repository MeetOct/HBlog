using Cheergo.AspNetCore.Extensions;
using HBlog.Web.Middlewares;
using HBlog.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace HBlog.Web
{
	public class Startup
    {
		private IConfiguration config;
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
        {
			services.AddConfiguration(out config,"configs/app");
			services.AddMvc();
			services.AddSingleton<IConfiguration>(config);
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Administrator", policy => policy.RequireRole("admin"));
				//options.AddPolicy("AgeOver25", policy => policy.Requirements.Add(new AgeAuthorizationRequirement(25)));
			});
			services.AddDbContext<BlogContext>(x => x.UseSqlite(config["DBFile"]));
			services.Inject(Assembly.Load(new AssemblyName("HBlog.Service")), (type) => type.Name.EndsWith("Service"));
			//services.AddScoped<RedisContext, RedisContext>();
			//RedisManager.InitConfig(config["RedisHost"]);
			//services.AddSingleton<IAuthorizationHandler, AgeAuthorizationHandler>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, BlogContext context)
        {
            loggerFactory.AddConsole();
			app.UseStaticFiles();
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				AuthenticationScheme =config["CookieName"],
				LoginPath = new PathString("/Admin/Login/"),
				//未授权
				AccessDeniedPath = new PathString("/Admin/Login/"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true,

				CookieHttpOnly=true,
				ExpireTimeSpan=DateTime.Now.AddDays(1).TimeOfDay
			});

			app.UseMiddleware<RequestLoggerMiddleware>();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
			DbInitializer(context);
		}


		public void DbInitializer(BlogContext context)
		{
			context.Database.EnsureCreated();
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
