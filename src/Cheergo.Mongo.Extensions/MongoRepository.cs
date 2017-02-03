using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Cheergo.Mongo.Extensions
{
	public class MongoRepository<TDocument> where TDocument : BaseEntity
	{
		private IMongoCollection<TDocument> mongoCollection;
		private static MongoClient client;
		public MongoRepository()
		{
			var typeInfo = typeof(TDocument).GetTypeInfo();
			var targetType = typeof(MongoAttribute);
			if (!typeInfo.IsDefined(targetType, false))
			{
				throw new Exception("实体未配置Mongo特性");
			}
			var attr = typeInfo.GetCustomAttribute(targetType) as MongoAttribute;
			client = MongoManager.GetClient();
			var data = client.GetDatabase(attr.Database);
			mongoCollection = data.GetCollection<TDocument>(attr.Collection);
		}

		public MongoRepository(string database, string collection)
		{
			client = MongoManager.GetClient();
			mongoCollection = client.GetDatabase(database).GetCollection<TDocument>(collection);
		}

		public void InsertOne(TDocument entity)
		{
			mongoCollection.InsertOne(entity);
		}

		public TDocument FindFirstOne(Expression<Func<TDocument, bool>> expression)
		{
			return mongoCollection.Find(Builders<TDocument>.Filter.Where(expression)).FirstOrDefault();
		}
	}
}
