using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheergo.Mongo.Extensions
{
    public class MongoManager
    {
		private static MongoClient client;
		private static object obj = new object();
		private static string[] host = null;

		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="_host">host:port</param>
		public static void InitHost(params string[] _host)
		{
			host = _host;
		}
		internal static MongoClient GetClient()
		{
			if(client==null)
			{
				lock (obj)
				{
					if(client == null)
					{
						if (host == null||!host.Any())
						{
							throw new Exception("请配置Mongdb连接字符串");
						}
						var connStr = new StringBuilder();
						connStr.Append("mongodb://");
						host.ToList().ForEach(h => connStr.Append($"{h},"));
						client = new MongoClient(connStr.ToString().TrimEnd(','));
					}
				}
			}
			return client;
		}

	}
}
