using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Authorization.Handlers
{
    public class TeamOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Team>
    {
		UserManager<User> _userManager;

		public TeamOwnerAuthorizationHandler(UserManager<User>
			userManager)
		{
			_userManager = userManager;
		}

		protected override Task
			HandleRequirementAsync(AuthorizationHandlerContext context,
								   OperationAuthorizationRequirement requirement,
								   Team resource)
		{
			Contract.Ensures(Contract.Result<Task>() != null);
			if (context.User == null || resource == null)
			{
				return Task.FromResult(0);
			}

			// If we're not asking for CRUD permission, return.

			if (requirement.Name != Constants.CreateOperationName &&
				requirement.Name != Constants.ReadOperationName &&
				requirement.Name != Constants.UpdateOperationName &&
				requirement.Name != Constants.DeleteOperationName)
			{
				return Task.FromResult(0);
			}

			// If User is the Owner then we are good
			if (resource.OwnerId == _userManager.GetUserId(context.User))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}


	public class TeamAdministraitorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Team>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
													   OperationAuthorizationRequirement requirement,
													   Team resource)
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


	public class TeamMembersAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Team>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
								   OperationAuthorizationRequirement requirement,
								   Team resource)
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
