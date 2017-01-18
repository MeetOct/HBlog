using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Web.Models
{
	public class BlogContext : DbContext
	{
		public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
		}

		public DbSet<Blog> Blogs { get; set; }
		public DbSet<BlogTag> BlogTags { get; set; }
		public DbSet<Catalog> Catalogs { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			
			builder.Entity<Blog>().ToTable("Blog");

			builder.Entity<BlogTag>().ToTable("BlogTag");

			builder.Entity<Catalog>().ToTable("Catalog");
		}
	}
}
