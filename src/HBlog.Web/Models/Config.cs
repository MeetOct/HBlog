using Cheergo.Redis.Extensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HBlog.Web.Models
{
	public class Config
    {
		[Required]
		[RedisHash]
		public string Account { get; set; }

		[Required]
		[RedisHash]
		public string Password { get; set; }
	}
}
