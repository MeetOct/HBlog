using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Cheergo.Npgsql.Extensions
{
	public class NpgsqlManager
	{
		private static string _host = string.Empty;
		private static Dictionary<string, Tuple<string, string>> _config;

		/// <summary>
		/// 程序启动时配置
		/// </summary>
		/// <param name="host">eg:127.0.0.1</param>
		/// <param name="config">配置数据库访问：<Database,<Username,Password>></param>
		public static void Init(string host,Dictionary<string,Tuple<string,string>> config)
		{
			_host = host;
			_config = config;
		}

		public static string GetConnectionString(string database)
		{
			return $"Host={_host};Username={_config[database].Item1};Password={_config[database].Item2};Database={database}";
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class NpgsqlAttribute : Attribute
	{
		public string Database { get; set; } = string.Empty;
		public string Table { get; set; } = string.Empty;
		public NpgsqlAttribute(string database, string table)
		{
			Database = database;
			Table = table;
		}
	}
	public class NpgsqlDAO<Entity> where Entity : class, new()
	{
		private string _connectionString = string.Empty;
		public NpgsqlDAO()
		{
			var attr = typeof(Entity).GetTypeInfo().GetCustomAttribute<NpgsqlAttribute>();
			if(attr==null)
			{
				throw new Exception("实体未配置Npgsql特性");
			}
			_connectionString = NpgsqlManager.GetConnectionString(attr.Database);
		}
		public void Execute(string sql,Dictionary<string,Tuple<dynamic,DbType>> parameters)
		{
			using (var conn = new NpgsqlConnection(_connectionString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = sql;	// $"INSERT INTO user_tbl (name,signup_date) reader (@name,@date)";
					foreach (var item in parameters)
					{
						cmd.Parameters.Add(new NpgsqlParameter() { ParameterName= item.Key, Value= item.Value.Item1,DbType= item.Value.Item2});
					}
					cmd.Prepare();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public List<Entity> QueryList(string sql, Dictionary<string, Tuple<dynamic, DbType>> parameters)
		{
			using (var conn = new NpgsqlConnection(_connectionString))
			{
				conn.Open();
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = sql;  // $"INSERT INTO user_tbl (name,signup_date) reader (@name,@date)";
					foreach (var item in parameters)
					{
						cmd.Parameters.Add(new NpgsqlParameter() { ParameterName = item.Key, Value = item.Value.Item1, DbType = item.Value.Item2 });
					}
					cmd.Prepare();
					using (var reader = cmd.ExecuteReader())
					{
						PropertyInfo[] properties = typeof(Entity).GetProperties();
						var result = new List<Entity>();
						while (reader.Read())
						{
							var entity = new Entity();
							var fieldCount = reader.FieldCount;
							for (int i = 0; i < fieldCount; i++)
							{
								var value= reader[i];
								if (value == null)
								{
									continue;
								}
								var property = properties.First(p => p.Name.Equals(reader.GetName(i),StringComparison.CurrentCultureIgnoreCase));
								if (property == null)
								{
									continue;
								}
								var setter = ReappearMember.CreatePropertySetter(property);
								//这里其实可能有很多类型需要判断
								if (property.PropertyType.GetTypeInfo().IsValueType)
								{
									switch (property.PropertyType.FullName)
									{
										case "System.Int64": setter(entity, Convert.ToInt64(value)); break;
										case "System.Int32": setter(entity, Convert.ToInt32(value)); break;
										case "System.Boolean": setter(entity, Convert.ToBoolean(value)); break;
										case "System.DateTime": setter(entity, Convert.ToDateTime(value)); break;
										case "System.Date": setter(entity, Convert.ToDateTime(value)); break;
										case "System.Decimal": setter(entity, Convert.ToDecimal(value)); break;
										case "System.Double": setter(entity, Convert.ToDouble(value)); break;
										case "System.Char": setter(entity, Convert.ToChar(value)); break;
										case "System.ToByte": setter(entity, Convert.ToByte(value)); break;
										case "System.Single": setter(entity, Convert.ToSingle(value)); break;
										default: break;
									}
								}
								else
								{
									setter(entity, value);
								}
							}
							result.Add(entity);
						}
						return result;
					}
				}
			}
		}
	}

	public class ReappearMember
	{
		public static Action<object, object> CreatePropertySetter(PropertyInfo property)
		{
			if (property == null)
				throw new ArgumentNullException("property");
			if (!property.CanWrite)
				return null;

			MethodInfo setMethod = property.GetSetMethod();
			DynamicMethod dm = new DynamicMethod("PropertySetter", null, new Type[] { typeof(object), typeof(object) }, property.DeclaringType, true);

			ILGenerator il = dm.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			Type type = property.PropertyType;
			if (type.GetTypeInfo().IsValueType)
				il.Emit(OpCodes.Unbox_Any, type);
			else
				il.Emit(OpCodes.Castclass, type);
			if (property.DeclaringType.GetTypeInfo().IsValueType)
				il.EmitCall(OpCodes.Call, setMethod, null);
			else
				il.EmitCall(OpCodes.Callvirt, setMethod, null);
			il.Emit(OpCodes.Ret);

			return dm.CreateDelegate(typeof(Action<object, object>)) as Action<object, object>;
		}

		public static Func<object, object> CreatePropertyGetter(PropertyInfo property)
		{
			if (property == null)
				throw new ArgumentNullException("property");
			if (!property.CanWrite)
				return null;

			MethodInfo getMethod = property.GetGetMethod();
			DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(object), new Type[] { typeof(object) }, property.DeclaringType, true);

			ILGenerator il = dm.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			if (property.DeclaringType.GetTypeInfo().IsValueType)
				il.EmitCall(OpCodes.Call, getMethod, null);
			else
				il.EmitCall(OpCodes.Callvirt, getMethod, null);
			Type type = property.PropertyType;
			if (type.GetTypeInfo().IsValueType)
				il.Emit(OpCodes.Box, type);
			else
				il.Emit(OpCodes.Castclass, type);
			il.Emit(OpCodes.Ret);

			return dm.CreateDelegate(typeof(Func<object, object>)) as Func<object, object>;
		}
	}
}
