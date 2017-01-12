using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cheergo.AspNetCore.Controllers;
using Microsoft.AspNetCore.Authorization;
using HBlog.Web.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HBlog.Web.Controllers
{
    public class BlogController : BaseController
	{
		private RedisContext _context;
		public BlogController(RedisContext context)
		{
			_context = context;
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Blog/Add")]
		public IActionResult EditBlog()
		{
			return View();
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Blog/Edit")]
		[HttpPost]
		public IActionResult EditBlog(BlogModel model)
		{
			if (!ModelState.IsValid)
			{
				Result.Message = "请完善文章";
				return Json(Result);
			}
			_context.AddBlog(model);
			Result.Message = "编辑成功";
			Result.Succeed = true;
			return Json(Result);
		}
	}
}
