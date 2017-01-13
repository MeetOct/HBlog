using Cheergo.Redis.Extensions.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HBlog.Core.Models
{
	public class BlogModel
	{
		[RedisHash]
		public long Id { get; set; } = 0;

		[RedisHash]
		[Required]
		public string Title { get; set; } = string.Empty;

		[RedisHash]
		[Required]
		public string Slug { get; set; } = string.Empty;

		[RedisHash]
		[Required]
		public string Content { get; set; } = string.Empty;

		[RedisHash]
		public DateTime Time { get; set; } = DateTime.Now;
	}
}
