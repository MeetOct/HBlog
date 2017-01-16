using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cheergo.AspNetCore.Controllers
{
	public class CheergoController : Controller
	{
		public AjaxResult Result { get; set; } = new AjaxResult();
		public IConfiguration Config { get { return HttpContext.RequestServices?.GetService<IConfiguration>(); } }
	}
}
