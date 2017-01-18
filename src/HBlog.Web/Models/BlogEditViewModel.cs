using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Web.Models
{
    public class BlogEditViewModel
    {
		public long Id { get; set; }

		[Required]
		public string Title { get; set; } = string.Empty;

		[MaxLength(50)]
		[Required]
		public string Slug { get; set; } = string.Empty;

		[Required]
		public string Content { get; set; } = string.Empty;

		public int CatalogId { get; set; }

		public List<string> Tags { get; set; } = new List<string>();
	}
}
