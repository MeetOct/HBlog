using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cheergo.AspNetCore.Controllers
{
    public class PageResult<T>
    {
		public int TotalCount { get; set; }

		public List<T> List { get; set; }
    }
}
