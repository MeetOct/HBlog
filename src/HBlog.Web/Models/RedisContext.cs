using Cheergo.Redis.Extensions;
using Cheergo.Redis.Extensions.Extensions;

namespace HBlog.Web.Models
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
		private long CreateId(string key)
		{
			return redisClient.Body.IncrementValue(key);
		}

		public void AddBlog(BlogModel model)
		{
			model.Id = CreateId(RedisId.Blog.ToString());
			redisClient.Body.StoreHashEntity(model.Key, model);
		}
	}
}
