using ServiceStack.Redis;
using System;

namespace Cheergo.Redis.Extensions
{
	public class RedisUnitOfWork : IUnitOfWork
	{
		private IRedisClient _redisContext;
		private IRedisTransaction _tran;

		public RedisUnitOfWork(IRedisClient client)
		{
			_redisContext = client;
			_tran = client.CreateTransaction();
		}

		public void TranCommand(Action<IRedisClient> command)
		{
			_tran.QueueCommand(command);
		}

		public void TranCommand(Action<IRedisClient> command, Action onSuccessCallback)
		{
			_tran.QueueCommand(command, onSuccessCallback);
		}

		public void Commit()
		{
			_tran.Commit();
		}

		public void Dispose()
		{
			_tran.Dispose();
			_redisContext.Dispose();
		}

		public void Roolback()
		{
			_tran.Rollback();
		}
	}
}
