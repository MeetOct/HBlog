using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HBlog.Web.Controllers
{
    public class HomeController : BaseController
	{
		// GET: /<controller>/

		[Authorize(Policy = "Administrator")]
		[Authorize(Policy = "AgeOver25")]
		public IActionResult Index()
        {
            return View();
        }

		[Route("Account/Login")]
		public IActionResult Login()
		{
			var principal = new ClaimsPrincipal();
			principal.AddIdentity(new ClaimsIdentity(new List<Claim>()
			{
				new Claim("name","hance"),
				new Claim("age","25"),
				new Claim("role","admin"),
				//new Claim("role","hance")
			}, "role", "name", "role"));

			HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", principal);
			return View();
		}
	}
}
