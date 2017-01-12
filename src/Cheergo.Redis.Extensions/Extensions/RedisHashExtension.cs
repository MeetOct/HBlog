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
		public static void StoreHashEntity<T>(this IRedisClient client, string key, T entity)
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
			client.SetRangeInHash(key, keyValuePairs);
		}

		public static T GetHashEntity<T>(this IRedisClient client, string key) where T : class, new()
		{
			var entity = new T();
			PropertyInfo[] properties = typeof(T).GetProperties();
			var values = client.GetAllEntriesFromHash(key);
			foreach (var property in properties)
			{
				if (property.IsDefined(typeof(RedisHashAttribute), false) && values.ContainsKey(property.Name))
				{
					var setter = ReappearMember.CreatePropertySetter(property);
					//这里其实可能有很多类型需要判断
					if (property.PropertyType.GetTypeInfo().IsValueType)
					{
						setter(entity, Convert.ToInt32(values[property.Name]));
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
