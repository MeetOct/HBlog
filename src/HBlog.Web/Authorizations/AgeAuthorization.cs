using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HBlog.Web.Authorizations
{
	public class AgeAuthorizationRequirement : IAuthorizationRequirement
	{
		public int Age { get; set; }
		public AgeAuthorizationRequirement(int age)
		{
			Age = age;
		}
	}

	public class AgeAuthorizationHandler : AuthorizationHandler<AgeAuthorizationRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeAuthorizationRequirement requirement)
		{
			if (!context.User.HasClaim(c => c.Type.ToLower() == "age"))
			{
				// .NET 4.x -> return Task.FromResult(0);
				return Task.CompletedTask;
			}
			int age = 0;
			if(!int.TryParse(context.User.FindFirst(c => c.Type.ToLower() == "age").Value, out age))
			{
				return Task.CompletedTask;
			}
			if (age >= requirement.Age)
			{
				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}

}
