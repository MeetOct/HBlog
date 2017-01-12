namespace Cheergo.Redis.Extensions
{
	public class RedisStringRepository : IRepository
	{
		private IUnitOfWork _unitOfWork;
		public RedisStringRepository(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public bool Set(string key, string value)
		{
			var succeed = false;
			_unitOfWork.TranCommand(e => e.Set<string>(key, value), () => succeed = true);
			return succeed;
		}
	}
}
