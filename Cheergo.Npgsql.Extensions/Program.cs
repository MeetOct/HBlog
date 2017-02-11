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
	[Npgsql("postgres", "user_tbl")]
	public class User
	{
		public string Name { get; set; } = "陌生人";

		public DateTime Signup_date { get; set; }
	}
	class Program
	{
		static void Main(string[] args)
		{
			var dic = new Dictionary<string, Tuple<string, string>>();
			dic.Add("postgres", new Tuple<string, string>("postgres", "ceshi"));
			NpgsqlManager.Init("120.77.65.127", dic);

			var dao = new NpgsqlDAO<User>();

			//var pars = new Dictionary<string, Tuple<dynamic, DbType>>();
			//pars.Add("name", new Tuple<dynamic, DbType>("测试", DbType.String));
			//pars.Add("date", new Tuple<dynamic, DbType>(DateTime.Now.ToString("yyyy-MM-dd"), DbType.Date));
			//dao.Execute($"INSERT INTO user_tbl (name,signup_date) VALUES (@name,@date)", pars);

			//var pars = new Dictionary<string, Tuple<dynamic, DbType>>();
			//pars.Add("name", new Tuple<dynamic, DbType>("新测试", DbType.String));
			//pars.Add("oldName", new Tuple<dynamic, DbType>("测试", DbType.String));
			//dao.Execute($"UPDATE user_tbl set name=@name where name=@oldName", pars);

			var pars = new Dictionary<string, Tuple<dynamic, DbType>>();
			pars.Add("name", new Tuple<dynamic, DbType>("hance", DbType.String));
			var list = dao.QueryList($"select Signup_date from user_tbl where name=@name", pars);
			list.ForEach(l => Console.WriteLine($"{l.Name}:{l.Signup_date}"));

			//using (var db = new NpgsqlContext())
			//{
			//	db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
			//	var count = db.SaveChanges();
			//	Console.WriteLine("{0} records saved to database", count);

			//	Console.WriteLine();
			//	Console.WriteLine("All blogs in database:");
			//	foreach (var blog in db.Blogs)
			//	{
			//		Console.WriteLine(" - {0}", blog.Url);
			//	}
			//}


			Console.ReadKey();
		}
	}
}
