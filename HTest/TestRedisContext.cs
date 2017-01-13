using Cheergo.Redis.Extensions;
using HBlog.Core.Models;
using System;
using Xunit;

namespace HTest
{
	public class TestRedisContext
	{
		public RedisContext context;
		public TestRedisContext()
		{
			RedisManager.InitConfig("hance1993@120.77.65.127:6379");
			context = new RedisContext();
		}

		[Fact]
		public void TestAddBlog()
		{
			var blog = new BlogModel()
			{
				Slug= $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}_测试缩略名",
				Title = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}_测试标题",
				Content = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}_测试内容",
				Time = DateTime.Now
			};

			var tuple= context.AddBlog(blog);
			Assert.True(tuple.Item1);

			var tuple2 = context.AddBlog(blog);
			Assert.False(tuple.Item1);
		}

		[Fact]
		public void TestUpdateBlog()
		{
			var model = context.GetBlog(11);
			model.Slug = "test-blog-update";
			var tuple = context.UpdateBlog(model);
			Assert.True(tuple.Item1);
		}
	}
}
