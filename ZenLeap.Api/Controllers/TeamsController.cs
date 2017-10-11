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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZenLeap.Api.Controllers
{
    [Route("api/[controller]")]
    public class TeamsController : BaseController
    {
		public TeamsController(DataContext context,
			IAuthorizationService authorizationService,
			UserManager<User> userManager)
            : base(context, authorizationService, userManager)
        {
		}

		/// <summary>
		/// GET: api/teams
		/// </summary>
		/// <returns>All Teams</returns>
		[HttpGet]
		public async Task<IEnumerable<Team>> Get()
		{
			if (User == null)
			{
				return null; // TODO: Need a better way to handle this. Maybe an exepection or redirect to login page?
			}

			var teams = await _unitOfWork.TeamRepository.GetAllAsync();

			var isGlobalAdmin = User.IsInRole(Constants.GlobalAdministratorsRole);

			var currentUserId = _userManager.GetUserId(User);

			if (!isGlobalAdmin)
			{
				teams = teams.Where(t => t.OwnerId == currentUserId);
			}

			return teams;
		}

		// TODO: implement security operations. Look into Claims
		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int? id)
		{
			// Ensure id was entered
			if (id == null)
			{
				return NotFound();
			}

			var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);

			// Ensure value was returned
			if (team == null)
			{
				return NotFound();
			}

			// Is authorized to Read
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, team, TeamOperations.Read);  

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

			return Ok(team);
		}

		// POST api/values
		[HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
