using Cheergo.Redis.Extensions;
using Cheergo.Redis.Extensions.Extensions;
using System;

namespace HBlog.Core.Models
{
	public class RedisContext
	{
		private RedisClientProxy redisClient;
		public RedisContext()
		{
			redisClient = RedisManager.GetClient();
		}

		/// <summary>
		/// 获取主键
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private long CreateIncrementId(string key)
		{
			return redisClient.Body.IncrementValue(key);
		}

		private bool IfNotExistsInHash(string hId,string key,string value)
		{
			return redisClient.Body.SetEntryInHashIfNotExists(hId, key, value);
		}

		#region Blog
		public Tuple<bool, string> AddBlog(BlogModel model)
		{
			var id = CreateIncrementId(RedisNames.BlogId.ToString());
			if (id <= 0)
			{
				return new Tuple<bool, string>(false, "新增失败");
			}
			model.Id = id;
			if (!IfNotExistsInHash(RedisNames.SlugToBlog.ToString(), model.Slug, model.Id.ToString()))
			{
				return new Tuple<bool, string>(false, "缩略名已存在");
			}
			redisClient.Body.StoreHashEntity($"{RedisNames.Blogs.ToString()}:{id}", model);
			return new Tuple<bool, string>(true, "新增成功");
		}

		public Tuple<bool, string> UpdateBlog(BlogModel model)
		{
			var oldSlug = redisClient.Body.GetValueFromHash($"{RedisNames.Blogs.ToString()}:{model.Id}", "Slug");
			if (!oldSlug.Equals(model.Slug))
			{
				if (!IfNotExistsInHash(RedisNames.SlugToBlog.ToString(), model.Slug, model.Id.ToString()))
				{
					return new Tuple<bool, string>(false, "缩略名已存在");
				}
				redisClient.Body.RemoveEntryFromHash(RedisNames.SlugToBlog.ToString(), oldSlug);
			}
			redisClient.Body.StoreHashEntity($"{RedisNames.Blogs.ToString()}:{model.Id}", model);
			return new Tuple<bool, string>(true, "新增成功");
		}

		public BlogModel GetBlog(long id)
		{
			return redisClient.Body.GetHashEntity<BlogModel>($"{RedisNames.Blogs.ToString()}:{id}");
		} 

		#endregion
	}
}
