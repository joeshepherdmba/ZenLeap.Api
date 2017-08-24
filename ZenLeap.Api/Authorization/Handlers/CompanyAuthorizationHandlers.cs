using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using ZenLeap.Api.Models;

namespace ZenLeap.Api.Authorization.Handlers
{
    public class CompanyIsOwnerAuthorizationHandler: AuthorizationHandler<OperationAuthorizationRequirement, Company>
    {
		UserManager<User> _userManager;

		public CompanyIsOwnerAuthorizationHandler(UserManager<User>
			userManager)
		{
			_userManager = userManager;
		}

		protected override Task
			HandleRequirementAsync(AuthorizationHandlerContext context,
								   OperationAuthorizationRequirement requirement,
								   Company resource)
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

            if (resource.OwnerId == int.Parse(_userManager.GetUserId(context.User)))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
    }

	public class CompanyAdministraitorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Company>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
													   OperationAuthorizationRequirement requirement,
													   Company resource)
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

	public class CompanyManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Company>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
								   OperationAuthorizationRequirement requirement,
								   Company resource)
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
			if (context.User.IsInRole(Constants.CompanyManagersRole))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}
}
