using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Cheergo.AspNetCore.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection Inject(this IServiceCollection services, Assembly assembly, Func<Type, bool> func = null)
		{
			var types = assembly.GetTypes().AsEnumerable();

			if (func != null)
			{
				types = types.Where(func);
			}

			foreach (var type in types)
			{
				var typeInfo = type.GetTypeInfo();
				var attribute = typeInfo.GetCustomAttribute(typeof(Attributes.InjectionAttribute), true) as Attributes.InjectionAttribute;
				if (attribute != null)
				{
					var descriptor = new ServiceDescriptor(attribute.ServiceType, type, attribute.Lifetime);
					services.Add(descriptor);
				}
			}
			return services;
		}

	}
}
