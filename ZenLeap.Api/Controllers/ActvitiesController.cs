using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZenLeap.Api.Data;
using ZenLeap.Api.Models;
using ZenLeap.Api.Authorization;
using System.Collections.Generic;

namespace ZenLeap.Api.Controllers
{
    [Route("api/[controller]")]
    public class ActvitiesController: BaseController
    {
		public ActvitiesController(DataContext context,
			IAuthorizationService authorizationService,
			UserManager<User> userManager)
            : base(context, authorizationService, userManager)
        {
        }


        /// <summary>
        /// GET: api/activities
        /// </summary>
        /// <returns>All Activities</returns>
        [HttpGet]
        public async Task<IEnumerable<Activity>> Get()
        {
            if (User == null)
            {
                return null; // TODO: Need a better way to handle this. Maybe an exepection or redirect to login page?
            }

            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();

            var isGlobalAdmin = User.IsInRole(Constants.GlobalAdministratorsRole);

            var currentUserId = _userManager.GetUserId(User);

            if (!isGlobalAdmin)
            {
                activities = activities.Where(a => a.Owner.Id == currentUserId);
            }

            return activities;
        }

		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int? id)
		{
			// Ensure id was entered
			if (id == null)
			{
				return NotFound();
			}

			var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);

			// Ensure value was returned
			if (activity == null)
			{
				return NotFound();
			}

			// Is authorized to Read
			var isAuthorized = await _authorizationService.AuthorizeAsync(User, activity, ActivityOperations.Read);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

			return Ok(activity);
		}

	}
}
