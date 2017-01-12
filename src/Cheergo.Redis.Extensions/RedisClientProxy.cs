using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheergo.Redis.Extensions
{
    public class RedisClientProxy
	{
		private IRedisClient _client;
		public RedisClientProxy(IRedisClient client)
		{
			_client = client;
		}

		public IRedisClient Body { get { return _client; } }
	}
}
