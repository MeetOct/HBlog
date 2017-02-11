using Cheergo.Mongo.Extensions;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Cheerego.Mongo.Extensions
{
	public class Program
    {
        public static void Main(string[] args)
        {
			#region Mongo
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


			//MongoManager.InitHost("120.77.65.127:27017");

			//var repository = new MongoRepository<UserEntity>();

			//repository.InsertOne(new UserEntity()
			//{
			//	Name = "cece",
			//	Age = 28,
			//	Stature = 1.8,
			//	School = new School() { Name = "安徽大学" }
			//});

			//repository.InsertOne(new UserEntity()
			//{
			//	Name = "cece",
			//	Age = 27,
			//	Stature = 1.9,
			//	School = new School() { Name = "安徽大学" }
			//});


			//Console.WriteLine(repository.DeleteOne(u => u.Name == "cece"));
			//Console.WriteLine(repository.DeleteList(u => u.Name == "cece"));
			//var user = repository.PageList(1,1,u => u.Name == "cece"&&u.School.Name=="安徽大学",new Dictionary<string, bool>()
			//{
			//	{ "Stature",false },
			//	{"Age",true }
			//});
			//Console.WriteLine(user.ToJson()); 
			#endregion


			using (var db = new BloggingContext())
			{
				db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
				var count = db.SaveChanges();
				Console.WriteLine("{0} records saved to database", count);

				Console.WriteLine();
				Console.WriteLine("All blogs in database:");
				foreach (var blog in db.Blogs)
				{
					Console.WriteLine(" - {0}", blog.Url);
				}
			}

			Console.ReadKey();
		}
    }
}
