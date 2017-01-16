using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Web.Models
{
    public class Config
    {
		[Required]
		public string Account { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		[Required]
		public string AvatarUrl { get; set; } = string.Empty;
	}
}
