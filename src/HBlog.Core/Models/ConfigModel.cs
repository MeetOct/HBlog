using Cheergo.Redis.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Core.Models
{
    public class ConfigModel
    {
		[Required]
		[RedisHash]
		public string Account { get; set; }

		[Required]
		[RedisHash]
		public string Password { get; set; }
	}
}
