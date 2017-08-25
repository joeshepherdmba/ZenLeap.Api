using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZenLeap.Api.Data;
using ZenLeap.Api.Models;
using ZenLeap.Api.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZenLeap.Api.Controllers
{
    [Route("api/[controller]")]
    public class CompaniesController : BaseController
    {
		public CompaniesController(DataContext context,
			IAuthorizationService authorizationService,
			UserManager<User> userManager)
            :base(context, authorizationService, userManager)
        {
            
        }

		// GET: api/companies        
		[HttpGet]
		public async Task<IEnumerable<Company>> Get()
		{
			var companies = await _unitOfWork.CompanyRepository.GetAllAsync();

			var isAuthorized = User.IsInRole(Constants.CompanyManagersRole) ||
							   User.IsInRole(Constants.CompanyAdministratorsRole);

            if(User == null){
                return null; // TODO: Need a better way to handle this. Maybe an exepection or redirect to login page?
            }
            var currentUserId = int.Parse(_userManager.GetUserId(User));

			// Only approved contacts are shown UNLESS you're authorized to see them
			// or you are the owner.
			if (!isAuthorized)
			{
				companies = companies.Where(c => c.OwnerId == currentUserId);
			}

            return companies;
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int? id)
		{
            // Ensure id was entered
            if(id == null){
                return NotFound();
            }

			var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
			
            // Ensure value was returned
            if (company == null)
			{
				return NotFound();
			}

            // Is authorized to Read
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, company, CompanyOperations.Read);

            if(!isAuthorized.Succeeded){
                return new ChallengeResult();
            }

			// EXAMPLE CODE: https://github.com/aspnet/Docs/blob/master/aspnetcore/security/authorization/secure-data/samples/final/Controllers/ContactsController.cs
			//if (contact.Status != ContactStatus.Approved &&   // Not approved.
			//!isAuthorizedRead &&        // Don't own it.
			//!isAuthorizedApprove)       // Not a manager.

			return Ok(company);
		}

		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Company value)
		{
			if (value == null)
			{
				return BadRequest();
			}

			// Is authorized to Create
			var isAuthorized = await _authorizationService.AuthorizeAsync(User, value, CompanyOperations.Create);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

            await _unitOfWork.CompanyRepository.AddAsync(value); // TODO: Figure out how to return the Id of the new company
			_unitOfWork.CompanyRepository.SaveChanges();

			return Ok();
			//return CreatedAtRoute("GetBook", new { id = createdBook.Id }, createdBook);
		}

        // PUT api/values/5
        [HttpPut("{id}")]
        //[ValidateUserExists]
        public async Task<IActionResult> Put(int id, [FromBody]Company value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, company, CompanyOperations.Update);
            var isGlobalAdmin = User.IsInRole(Constants.GlobalAdministratorsRole);

            if (isAuthorized.Succeeded || isGlobalAdmin)
            {
                company.CompanyName = value.CompanyName;
                company.DateEstablished = value.DateEstablished;
                company.Owner = value.Owner;
                company.OwnerId = value.OwnerId;
                company.Projects = value.Projects;

                _unitOfWork.CompanyRepository.Update(company);
                _unitOfWork.CompanyRepository.SaveChanges();
                return Ok(company);
            }

            return Unauthorized();
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		//[ValidateUserExists]
		public async Task<IActionResult> Delete(int id)
		{
			var companyToDelete = _unitOfWork.CompanyRepository.GetById(id);

			var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);

			var isAuthorized = await _authorizationService.AuthorizeAsync(User, company, CompanyOperations.Update);

			if(!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

			_unitOfWork.CompanyRepository.Delete(companyToDelete);
			await _unitOfWork.CompanyRepository.SaveChangesAsync();
			return Ok();
		}
    }
}
