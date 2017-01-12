using ServiceStack.Redis;
using System;

namespace Cheergo.Redis.Extensions
{
	public interface IUnitOfWork : IDisposable
	{
		void TranCommand(Action<IRedisClient> command);

		void TranCommand(Action<IRedisClient> command, Action onSuccessCallback);

		void Commit();

		void Roolback();
	}
}
