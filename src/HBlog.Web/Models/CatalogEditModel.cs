using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Web.Models
{
    public class CatalogEditModel
    {
		public int Id { get; set; }

		[MaxLength(32)]
		[Required]
		public string TitleEn { get; set; } = string.Empty;

		[MaxLength(32)]
		[Required]
		public string TitleZh { get; set; } = string.Empty;

		public int Order { get; set; }
	}
}
