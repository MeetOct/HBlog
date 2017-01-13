using System;

namespace HBlog.Core.Attributes
{
	public class HashConfigAttribute : Attribute
	{
		public string Prefix { get; }
		public string IncrId { get; }
		public HashConfigAttribute(string prefix,string incrId)
		{
			Prefix = prefix;
			IncrId = incrId;
		}
	}
}
