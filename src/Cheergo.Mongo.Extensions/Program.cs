using Cheergo.Mongo.Extensions;
using System;

namespace Cheerego.Mongo.Extensions
{
	public class Program
    {
        public static void Main(string[] args)
        {
			//The MongoClient instance actually represents a pool of connections to the database; you will only need one instance of class MongoClient even with multiple threads.
			//var client = new MongoClient("mongodb://120.77.65.127:27017");
			//var database = client.GetDatabase("foo"); //database
			//var collection = database.GetCollection<BaseEntity>("bar"); //table

			//var doc = new TestClass
			//{
			//	Name="hance",
			//	Age=22
			//};
			//collection.InsertOne(doc);

			//var filter = Builders<BaseEntity>.Filter.Where(x => x.Age == 22);

			//var result =collection.Find(filter).FirstOrDefault();


			MongoManager.InitHost("120.77.65.127:27017");

			var repository = new MongoRepository<UserEntity>();

			repository.InsertOne(new UserEntity()
			{
				Name="hance",
				Age=24
			});

			repository.InsertOne(new UserEntity()
			{
				Name = "cece",
				Age = 23
			});

			var user = repository.FindFirstOne(u => u.Age == 24);
			//Console.WriteLine(user.ToJson());
			Console.ReadKey();
		}
    }
}
