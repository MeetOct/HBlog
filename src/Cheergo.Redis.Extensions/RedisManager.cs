using ServiceStack.Redis;

namespace Cheergo.Redis.Extensions
{
	public class RedisManager
	{
		private static PooledRedisClientManager clientManager;	
		private static object obj = new object();
		private static string[] host = null;

		public static void InitConfig(params string[] _host)
		{
			host = _host;
		}

		private static void CreateManager()
		{
			clientManager = new PooledRedisClientManager(host);
		}

		public static RedisClientProxy GetClient()
		{
			if (clientManager == null)
			{
				lock (obj)
				{
					if (clientManager == null)
					{
						CreateManager();
					}
				}
			}
			return new RedisClientProxy(clientManager.GetClient());
		}
	}
}
