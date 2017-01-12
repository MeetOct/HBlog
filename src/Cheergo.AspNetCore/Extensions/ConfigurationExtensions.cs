using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheergo.AspNetCore.Extensions
{
    public static class ConfigurationExtensions
    {
		public static IServiceCollection AddConfiguration(this IServiceCollection self, out IConfiguration config, string filePath = "config")
		{
			var services = self.BuildServiceProvider();
			var env = services.GetRequiredService<IHostingEnvironment>();

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile($"{filePath}.json")
				.AddJsonFile($"{filePath}.{env.EnvironmentName}.json", optional: true);
			config = builder.Build();
			self.AddSingleton<IConfiguration>(config);

			return self;
		}

		public static IServiceCollection AddConfiguration(this IServiceCollection self, string filePath = "config")
		{
			var services = self.BuildServiceProvider();
			var env = services.GetRequiredService<IHostingEnvironment>();

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile($"{filePath}.json")
				.AddJsonFile($"{filePath}.{env.EnvironmentName}.json", optional: true);
			var configuration = builder.Build();
			self.AddSingleton<IConfiguration>(configuration);

			return self;
		}
	}
}
