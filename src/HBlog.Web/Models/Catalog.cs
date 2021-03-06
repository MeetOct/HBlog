﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBlog.Web.Models
{
	public class Catalog
    {
		public int ID { get; set; }

		[MaxLength(32)]
		[Required]
		public string TitleEn { get; set; } = string.Empty;

		public string TitleZh { get; set; } = string.Empty;

		public int Order { get; set; }

		public virtual ICollection<Blog> Posts { get; set; }
	}
}
