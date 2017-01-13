using ServiceStack.Redis;
using System;

namespace Cheergo.Redis.Extensions
{
	//redis事务就是一个坑
	public class RedisUnitOfWork : IUnitOfWork
	{
		private IRedisClient _client;
		private IRedisTransaction _tran;

		public RedisUnitOfWork(IRedisClient client)
		{
			_client = client;
			_tran = _client.CreateTransaction();
		}

		public IRedisClient Client { get { return _client; } }

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
			_client.Dispose();
		}

		public void Roolback()
		{
			_tran.Rollback();
		}
	}
}
