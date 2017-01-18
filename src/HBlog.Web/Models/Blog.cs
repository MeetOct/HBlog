
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBlog.Web.Models
{
	public class Blog
    {
		public int ID { get; set; }

		[Required]
		public string Title { get; set; } = string.Empty;

		[MaxLength(50)]
		[Required]
		public string Slug { get; set; } = string.Empty;

		[Required]
		public string Summary { get; set; } = string.Empty;

		[Required]
		public string Content { get; set; } = string.Empty;

		public DateTime Time { get; set; }

		[ForeignKey("Catalog")]
		public int? CatalogID { get; set; }

		public Catalog Catalog { get; set; }

		public virtual ICollection<BlogTag> Tags { get; set; }

	}
}
