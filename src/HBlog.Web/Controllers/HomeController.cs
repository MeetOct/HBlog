using Cheergo.AspNetCore.Controllers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HBlog.Web.Controllers
{
	public class HomeController : BaseController
	{
		// GET: /<controller>/
		[Route("Home/Index")]
		public IActionResult Index()
        {
			return View();
        }
	}
}
