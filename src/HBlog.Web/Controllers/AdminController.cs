using Cheergo.AspNetCore.Controllers;
using Cheergo.Kernel.Extensions;
using HBlog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HBlog.Web.Controllers
{
	public class AdminController : BaseController
	{
		private RedisContext _context;
		public AdminController(RedisContext context)
		{
			_context = context;
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Index")]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Policy = "Administrator")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Admin/Config/Update")]
		public IActionResult UpdateConfig(Config config)
		{
			if (!ModelState.IsValid)
			{
				Result.Message = "请完善配置信息";
				return Json(Result);
			}
			Config["Account"] = config.Account;
			Config["Password"] = config.Password;
			Result.Succeed = true;
			Result.Message = "更新成功";
			return Json(Result);
		}

		[Route("Admin/Login")]
		public IActionResult Login()
		{
			return View();
		}

		[Route("Admin/Login")]
		[HttpPost]
		public async Task<IActionResult> Login(string account, string password)
		{
			if (account.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
			{
				Result.Message = "用户名或密码错误";
				return Json(Result);
			}
			if (account.Equals(Config["Account"]) && password.Equals(Config["Password"]))
			{
				Result.Message = "用户名或密码错误";
				return Json(Result);
			}
			await SignInAsync(account);
			Result.Message = "登录成功";
			Result.Succeed = true;
			return Json(Result);
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Logout")]
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.Authentication.SignOutAsync(Config["CookieName"]);
			Result.Message = "注销成功";
			Result.Succeed = true;
			return Json(Result);
		}

		private async Task SignInAsync(string account)
		{
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
			{
				new Claim("account",account),
				new Claim("role","admin"),
			}, "role", "name", "role"));
			await HttpContext.Authentication.SignInAsync(Config["CookieName"], principal);
		}
	}
}
