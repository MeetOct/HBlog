using Cheergo.Kernel.Reflection;
using Cheergo.Redis.Extensions.Attributes;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cheergo.Redis.Extensions.Extensions
{
	public static class RedisHashExtension
	{
		public static void StoreHashEntity<T>(this IRedisClient client,string hId,T entity)
		{
			var keyValuePairs = new List<KeyValuePair<string, string>>();
			PropertyInfo[] properties = typeof(T).GetProperties();

			foreach (var property in properties)
			{
				if (property.IsDefined(typeof(RedisHashAttribute), false))
				{
					var getter = ReappearMember.CreatePropertyGetter(property);
					keyValuePairs.Add(new KeyValuePair<string, string>(property.Name, getter(entity)?.ToString()));
				}
			}
			client.SetRangeInHash(hId, keyValuePairs);
		}

		public static T GetHashEntity<T>(this IRedisClient client, string hId) where T : class, new ()
		{
			var entity = new T();
			PropertyInfo[] properties = typeof(T).GetProperties();
			var values = client.GetAllEntriesFromHash(hId);
			foreach (var property in properties)
			{
				if (property.IsDefined(typeof(RedisHashAttribute), false) && values.ContainsKey(property.Name))
				{
					var setter = ReappearMember.CreatePropertySetter(property);
					//这里其实可能有很多类型需要判断
					if (property.PropertyType.GetTypeInfo().IsValueType)
					{
						switch (property.PropertyType.FullName)
						{
							case "System.Int64": setter(entity, Convert.ToInt64(values[property.Name]));break;
							case "System.Int32": setter(entity, Convert.ToInt32(values[property.Name]));break;
							case "System.Boolean": setter(entity, Convert.ToBoolean(values[property.Name]));break;
							case "System.DateTime": setter(entity, Convert.ToDateTime(values[property.Name]));break;
							case "System.Decimal": setter(entity, Convert.ToDecimal(values[property.Name]));break;
							case "System.Double": setter(entity, Convert.ToDouble(values[property.Name]));break;
							case "System.Char": setter(entity, Convert.ToChar(values[property.Name]));break;
							case "System.ToByte": setter(entity, Convert.ToByte(values[property.Name]));break;
							case "System.Single": setter(entity, Convert.ToSingle(values[property.Name]));break;
							default: break;
						}
					}
					else
					{
						setter(entity, values[property.Name]);
					}
				}
			}
			return entity;
		}
	}
}
