using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cheergo.AspNetCore.Controllers;
using HBlog.Web.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HBlog.Web.Controllers
{
    public class BaseController : CheergoController
	{
		public BlogContext DB;
		public BaseController(BlogContext db)
		{
			DB = db;
		}
	}
}
