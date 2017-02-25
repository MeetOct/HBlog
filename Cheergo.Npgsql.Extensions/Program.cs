using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cheergo.Npgsql.Extensions.NpgsqlContext;

namespace Cheergo.Npgsql.Extensions
{
	[Npgsql("postgres", "user_tb")]
	public class User
	{
		public string Name { get; set; } = "陌生人";

		public int Id { get; set; }
	}
	class Program
	{
		static void Main(string[] args)
		{
			//var dic = new Dictionary<string, Tuple<string, string>>();
			//dic.Add("postgres", new Tuple<string, string>("postgres", "password"));
			//NpgsqlManager.Init("120.77.65.127", dic);

			//var dao = new NpgsqlDAO<User>();

			//var pars = new Dictionary<string, Tuple<dynamic, DbType>>();
			//pars.Add("name", new Tuple<dynamic, DbType>("测试", DbType.String));
			//pars.Add("id", new Tuple<dynamic, DbType>(1, DbType.Int32));
			//dao.Execute($"INSERT INTO user_tb (name,id) VALUES (@name,@id)", pars);

			//var pars = new Dictionary<string, Tuple<dynamic, DbType>>();
			//pars.Add("name", new Tuple<dynamic, DbType>("新测试", DbType.String));
			//pars.Add("oldName", new Tuple<dynamic, DbType>("测试", DbType.String));
			//dao.Execute($"UPDATE user_tbl set name=@name where name=@oldName", pars);

			//var pars = new Dictionary<string, Tuple<dynamic, DbType>>();
			//pars.Add("name", new Tuple<dynamic, DbType>("测试", DbType.String));
			//var list = dao.QueryList($"select Id,name from user_tb where name=@name", pars);
			//list.ForEach(l => Console.WriteLine($"{l.Id}:{l.Name}"));

			using (var db = new NpgsqlContext())
			{
				db.User.Add(new User_tb { Name = "ceshi",Id=2 });
				var count = db.SaveChanges();
				Console.WriteLine("{0} records saved to database", count);

				Console.WriteLine();
				Console.WriteLine("All blogs in database:");
				foreach (var blog in db.User)
				{
					Console.WriteLine(" - {0}", blog.Name);
				}
			}


			Console.ReadKey();
		}
	}
}
