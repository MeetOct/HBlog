using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheergo.Redis.Extensions.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class RedisHashAttribute : Attribute
	{

	}
}
