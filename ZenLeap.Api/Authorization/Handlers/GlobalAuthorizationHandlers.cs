using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Authorization.Handlers
{
	public class GlobalAdministraitorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Team>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
													   OperationAuthorizationRequirement requirement,
													   Team resource)
		{
			if (context.User == null)
			{
				return Task.FromResult(0);
			}

			// Administrators can do anything.
			if (context.User.IsInRole(Constants.CompanyAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}
}
