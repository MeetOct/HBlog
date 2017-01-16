using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Web.Models
{
    public class BlogTag
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("Blog")]
		public long BlogId { get; set; }

		public virtual Blog Blog { get; set; }

		[MaxLength(64)]
		[Required]
		public string Tag { get; set; } = string.Empty;
	}
}
