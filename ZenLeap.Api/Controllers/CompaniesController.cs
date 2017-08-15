using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenLeap.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZenLeap.Api.Controllers
{
    [Route("api/[controller]")]
    public class CompaniesController : BaseController
    {
		// GET: api/companies        
		[HttpGet]
		public async Task<IEnumerable<Company>> Get()
		{
			return await _unitOfWork.CompanyRepository.GetAllAsync();
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
			if (company == null)
			{
				return NotFound();
			}
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

			await _unitOfWork.CompanyRepository.AddAsync(value);
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

            company.CompanyName = value.CompanyName;
            company.DateEstablished = value.DateEstablished;
            company.Owner = value.Owner;
            company.OwnerId = value.OwnerId;
            company.Projects = value.Projects;

			_unitOfWork.CompanyRepository.Update(company);
			_unitOfWork.CompanyRepository.SaveChanges();
			return Ok(company);
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		//[ValidateUserExists]
		public async Task<IActionResult> Delete(int id)
		{
			var userToDelete = _unitOfWork.CompanyRepository.GetById(id);
			_unitOfWork.CompanyRepository.Delete(userToDelete);
			await _unitOfWork.CompanyRepository.SaveChangesAsync();
			return Ok();
		}
    }
}
