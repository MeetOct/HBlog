using Cheergo.Redis.Extensions.Attributes;
using HBlog.Web.Attributes;
using ServiceStack.DataAnnotations;
using System;
using System.Reflection;

namespace HBlog.Web.Models
{
	[RedisKey("Blog")]
    public class BlogModel
    {
		[RedisHash]
		public long Id { get; set; } = 0;

		[RedisHash]
		[Required]
		public string Title { get; set; } = string.Empty;

		[RedisHash]
		[Required]
		public string Content { get; set; } = string.Empty;

		[RedisHash]
		public DateTime Time { get; set; } = DateTime.Now;

		public string Key
		{
			get
			{
				if (Id <= 0)
				{
					throw new Exception("Blog主键错误！");
				}
				var attr= typeof(BlogModel).GetTypeInfo().GetCustomAttribute(typeof(RedisKeyAttribute), true) as RedisKeyAttribute;
				return $"{attr.Key}:{Id}";
			}
		}
    }
}
