using Cheergo.AspNetCore.Controllers;
using HBlog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HBlog.Web.Controllers
{
	public class BlogController : BaseController
	{
		private BlogContext DB;
		public BlogController(BlogContext db)
		{
			DB = db;
		}

		#region Admin Action

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Blog/New")]
		[HttpPost]
		public IActionResult NewBlog()
		{
			var post = new Blog
			{
				Title = "新建博客",
				CatalogID = null,
				Slug = Guid.NewGuid().ToString().Replace("-", ""),
				Time = DateTime.Now,
			};
			DB.Blogs.Add(post);
			DB.SaveChanges();
			Result.ResultData = post;
			Result.Succeed = true;
			return Json(Result);
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Blog/Edit")]
		[HttpPost]
		public IActionResult EditBlog(BlogEditViewModel model)
		{
			if (!ModelState.IsValid)
			{
				Result.Message = "请完善博客信息";
				return Json(Result);
			}
			if (DB.Blogs.Any(x => x.ID != model.Id && x.Slug.Equals(model.Slug)))
			{
				Result.Message = "博客缩略名已存在，请修改";
				return Json(Result);
			}
			var blog = DB.Blogs
			   .Include(x => x.Tags)
			   .SingleOrDefault(x => x.ID == model.Id);

			if (blog == null || blog.ID <= 0)
			{
				Result.Message = "未找到博客信息";
				return Json(Result);
			}

			foreach (var t in blog.Tags)
			{
				DB.BlogTags.Remove(t);
			}
			if (model.Tags != null && model.Tags.Any())
			{
				foreach (var t in model.Tags)
				{
					blog.Tags.Add(new BlogTag { BlogID = blog.ID, Tag = t.Trim() });
				}
			}

			var summary = "";
			var flag = false;
			var tmp = model.Content.Split('\n');
			if (tmp.Count() > 10)
			{
				for (var i = 0; i < 10; i++)
				{
					if (tmp[i].IndexOf("```") == 0)
					{
						flag = !flag;
					}
					summary += tmp[i] + '\n';
				}
				if (flag)
				{
					summary += "```\r\n";
				}
			}
			else
			{
				summary = model.Content;
			}

			blog.Summary = summary;
			blog.Title = model.Title;
			blog.Slug = model.Slug;
			blog.Content = model.Content;
			blog.CatalogID = null;
			DB.SaveChanges();
			Result.Message = "编辑博客成功";
			Result.Succeed = true;
			return Json(Result);
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Blog/Delete")]
		[HttpPost]
		public IActionResult DeleteBlog(long id)
		{
			var blog = DB.Blogs
			   .Include(x => x.Tags)
			   .SingleOrDefault(x => x.ID == id);

			if (blog == null || blog.ID <= 0)
			{
				Result.Message = "未找到博客信息";
				return Json(Result);
			}
			foreach (var t in blog.Tags)
			{
				DB.BlogTags.Remove(t);
			}
			DB.Blogs.Remove(blog);
			DB.SaveChanges();
			Result.Message = "删除成功";
			Result.Succeed = true;
			return Json(Result);
		}

		[Authorize(Policy = "Administrator")]
		[ValidateAntiForgeryToken]
		[HttpPost]
		[Route("Admin/Catalog/New")]
		public IActionResult NewCatalog(CatalogEditModel model)
		{
			if (!ModelState.IsValid)
			{
				Result.Message = "请完善栏目信息";
				return Json(Result);
			}
			var catalog = new Catalog
			{
				Order = 0,
				TitleZh = "新建栏目",
				TitleEn = "New Catalog"
			};
			DB.Catalogs.Add(catalog);
			DB.SaveChanges();
			Result.Message = "新建栏目成功";
			Result.Succeed = true;
			return Json(Result);
		}

		[Authorize(Policy = "Administrator")]
		[ValidateAntiForgeryToken]
		[HttpPost]
		[Route("Admin/Catalog/Edit")]
		public IActionResult EditCatalog(CatalogEditModel model)
		{
			if (!ModelState.IsValid)
			{
				Result.Message = "请完善栏目信息";
				return Json(Result);
			}
			var catalog = DB.Catalogs.SingleOrDefault(x => x.ID == model.Id);
			if (catalog == null || catalog.ID <= 0)
			{
				Result.Message = "未找到栏目信息";
				return Json(Result);
			}
			catalog.Order = model.Order;
			catalog.TitleZh = model.TitleZh;
			catalog.TitleEn = model.TitleEn.ToLower();
			DB.SaveChanges();
			Result.Message = "编辑栏目成功";
			Result.Succeed = true;
			return Json(Result);
		}

		[Authorize(Policy = "Administrator")]
		[Route("Admin/Catalog/Delete")]
		[HttpPost]
		public IActionResult DeleteCatalog(int id)
		{
			var catalog = DB.Catalogs
									.Where(x => x.ID == id)
									.SingleOrDefault();

			if (catalog == null || catalog.ID <= 0)
			{
				Result.Message = "未找到栏目信息";
				return Json(Result);
			}
			DB.Catalogs.Remove(catalog);
			DB.SaveChanges();
			Result.Message = "删除成功";
			Result.Succeed = true;
			return Json(Result);
		}

		#endregion

		#region Everyone Action

		[Route("Blog/Page/{Page:int?}")]
		[HttpPost]
		public IActionResult Page(int page)
		{
			var lambda = DB.Blogs
									.Include(x => x.Tags)
									.Include(x => x.Catalog)
									.OrderByDescending(x => x.Time);
			var result = new PageResult<Blog>()
			{
				TotalCount = lambda.Count(),
				List = lambda.Skip((page - 1) * 5).Take(5).ToList()
			};
			result.List.ForEach(x => 
			{
				if (x.Tags.Any())
				{
					x.Tags.ToList().ForEach(y => y.Blog = null);
				}
			});
			return Json(result);
		}

		[Route("Blog/{id:int}")]
		[HttpPost]
		public IActionResult Blog(int id)
		{
			var entity = DB.Blogs
									.Include(x => x.Tags)
									.Include(x => x.Catalog)
									.SingleOrDefault(x => x.ID == id);
			entity.Tags.ToList().ForEach(y => y.Blog = null);
			return Json(entity);
		}

		[Route("Blog/{Slug}")]
		[HttpPost]
		public IActionResult Blog(string slug)
		{
			var entity = DB.Blogs
									.Include(x => x.Tags)
									.Include(x => x.Catalog)
									.SingleOrDefault(x => x.Slug.Equals(slug));
			return Json(entity);
		}

		[Route("{Year:int}/{Month:int}/{Page:int?}")]
		[HttpPost]
		public IActionResult Calendar(int year, int month, int page = 1)
		{
			var begin = new DateTime(year, month, 1);
			var end = begin.AddMonths(1);
			var lambda = DB.Blogs
				.Include(x => x.Tags)
				.Include(x => x.Catalog)
				.Where(x => x.Time >= begin && x.Time <= end)
				.OrderByDescending(x => x.Time);
			var result = new PageResult<Blog>()
			{
				TotalCount = lambda.Count(),
				List = lambda.Skip((page - 1) * 5).Take(5).ToList()
			};
			return Json(result);
		}

		[Route("Catalog/{En}/{Page:int?}")]
		[HttpPost]
		public IActionResult Catalog(string en, int page = 1)
		{
			var catalog = DB.Catalogs
				.Where(x => x.TitleEn == en.ToLower())
				.SingleOrDefault();
			if (catalog == null || catalog.ID <= 0)
			{
				return Json(new PageResult<Blog>());
			}

			var lambda = DB.Blogs
			.Include(x => x.Tags)
			.Include(x => x.Catalog)
			.Where(x => x.CatalogID == catalog.ID)
			.OrderByDescending(x => x.Time);
			var result = new PageResult<Blog>()
			{
				TotalCount = lambda.Count(),
				List = lambda.Skip((page - 1) * 5).Take(5).ToList()
			};
			return Json(result);
		}

		[Route("Tag/{Tag}/{Page:int?}")]
		[HttpPost]
		public IActionResult Tag(string tag, int page = 1)
		{
			var lambda = DB.Blogs
					.Include(x => x.Tags)
					.Include(x => x.Catalog)
					.Where(x => x.Tags.Any(t => t.Tag.Equals(tag)))
					.OrderByDescending(x => x.Time);
			var result = new PageResult<Blog>()
			{
				TotalCount = lambda.Count(),
				List = lambda.Skip((page - 1) * 5).Take(5).ToList()
			};
			return Json(result);
		}

		[Route("Search/{Title}/{Page:int?}")]
		[HttpPost]
		public IActionResult Search(string title, int page = 1)
		{
			var lambda = DB.Blogs
					.Include(x => x.Tags)
					.Include(x => x.Catalog)
					.Where(x => x.Title.Contains(title) || x.Slug.Contains(title))
					.OrderByDescending(x => x.Time);
			var result = new PageResult<Blog>()
			{
				TotalCount = lambda.Count(),
				List = lambda.Skip((page - 1) * 5).Take(5).ToList()
			};
			return Json(result);
		}

		#endregion
	}
}
