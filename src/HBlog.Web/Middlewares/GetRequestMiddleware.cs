using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBlog.Web.Middlewares
{
	public class RequestLoggerMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestLoggerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			if (context.Request.Method.ToLower() == "get")
			{
				//确保刷新页面时，单页路由进入正确的页面，所有ajax请求采用post方式
				context.Request.Path = "/Home/Index";
			}
			await _next.Invoke(context);
		}
	}
}
