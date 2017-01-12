using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cheergo.AspNetCore.Attributes
{
	public class InjectionAttribute: Attribute
	{
		public InjectionAttribute() : this(null) { }

		public InjectionAttribute(Type serviceType) : this(serviceType, ServiceLifetime.Transient) { }

		public InjectionAttribute(Type serviceType, ServiceLifetime lifetime)
		{
			ServiceType = serviceType;
			Lifetime = lifetime;
		}

		public Type ServiceType { get; }

		public ServiceLifetime Lifetime { get; }
	}
}
