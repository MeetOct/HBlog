using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheergo.Mongo.Extensions
{
	public class BaseEntity
	{
		/// <summary>
		/// 主键
		/// </summary>
		public ObjectId Id { get; set; }
	}

	[Mongo("test","user")]
    public class UserEntity: BaseEntity
	{
		public string Name { get; set; } = string.Empty;

		public int Age { get; set; } = 0;
    }
}
