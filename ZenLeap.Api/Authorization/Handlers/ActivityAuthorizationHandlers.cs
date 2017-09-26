using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Authorization.Handlers
{
	public class ActivityAdministraitorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Activity>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
													   OperationAuthorizationRequirement requirement,
													   Activity resource)
		{
			if (context.User == null)
			{
				return Task.FromResult(0);
			}

			// Global Administrators can do anything.
			if (context.User.IsInRole(Constants.GlobalAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			// Administrators can do anything.
			if (context.User.IsInRole(Constants.TeamAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}


	public class ActivityMembersAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Activity>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
								   OperationAuthorizationRequirement requirement,
								   Activity resource)
		{
			if (context.User == null || resource == null)
			{
				return Task.FromResult(0);
			}

			// If not asking for approval/reject, return.
			if (requirement.Name != Constants.ApproveOperationName &&
				requirement.Name != Constants.RejectOperationName)
			{
				return Task.FromResult(0);
			}

			// Managers can approve or reject.
			if (context.User.IsInRole(Constants.TeamMembersRole))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}
}
