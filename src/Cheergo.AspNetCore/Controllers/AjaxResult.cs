using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheergo.AspNetCore.Controllers
{
	public class AjaxResult
	{
		public bool Succeed { get; set; } = false;
		public string Message { get; set; } = string.Empty;

		public dynamic ResultData { get; set; }
	}
}
