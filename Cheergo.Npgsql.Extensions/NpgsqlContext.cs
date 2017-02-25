using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheergo.Npgsql.Extensions
{
	public class NpgsqlContext : DbContext
	{
		public DbSet<User_tb> User { get; set; }
		//public DbSet<Post> Posts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(@"Host=120.77.65.127;Username=postgres;Password=password;Database=postgres;");
		}

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	modelBuilder.Entity<Blog>(entity =>
		//	{
		//		entity.Property(e => e.Url).IsRequired();
		//	});

		//	modelBuilder.Entity<Post>(entity =>
		//	{
		//		entity.HasOne(d => d.Blog)
		//			.WithMany(p => p.Posts)
		//			.HasForeignKey(d => d.BlogId);
		//	});
		//}
	}

	public class User_tb
	{
		public string Name { get; set; } = "陌生人";

		public int Id { get; set; }
	}

	public class Blog
	{
		public int BlogId { get; set; }
		public string Url { get; set; }

		public List<Post> Posts { get; set; }
	}

	public class Post
	{
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }

		public int BlogId { get; set; }
		public Blog Blog { get; set; }
	}
}
