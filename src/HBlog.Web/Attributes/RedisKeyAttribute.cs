using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Web.Attributes
{
    public class RedisKeyAttribute: Attribute
	{
		public string Key { get; }
		public RedisKeyAttribute(string key)
		{
			Key = key;
		}
	}
}
