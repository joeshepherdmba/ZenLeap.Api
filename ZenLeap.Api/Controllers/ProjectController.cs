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
    public class ProjectController : BaseController
    {
		// GET: api/users        
		[HttpGet]
		public async Task<IEnumerable<Project>> Get()
		{
			return await _unitOfWork.ProjectRepository.GetAllAsync();
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
			if (project == null)
			{
				return NotFound();
			}
			return Ok(project);
		}

		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]Project value)
		{
			if (value == null)
			{
				return BadRequest();
			}

			await _unitOfWork.ProjectRepository.AddAsync(value);
			_unitOfWork.ProjectRepository.SaveChanges();

			return Ok();
			//return CreatedAtRoute("GetBook", new { id = createdBook.Id }, createdBook);
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody]Project value)
		{
			if (value == null)
			{
				return BadRequest();
			}
			var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);

            project.Title = value.Title;
            project.Company = value.Company;
            project.CompanyId = value.CompanyId;
            project.Description = value.Description;
            project.ProjectOwner = value.ProjectOwner;
            project.ProjectOwnerId = value.ProjectOwnerId;
            project.ProjectTasks = value.ProjectTasks;
            project.HealthFactor = value.HealthFactor;
            project.ProjectValue = value.ProjectValue;
            project.VelocityFactor = value.VelocityFactor;

			_unitOfWork.ProjectRepository.Update(project);
			_unitOfWork.ProjectRepository.SaveChanges();
			return Ok(project);
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		//[ValidateUserExists]
		public async Task<IActionResult> Delete(int id)
		{
			var userToDelete = _unitOfWork.ProjectRepository.GetById(id);
			_unitOfWork.ProjectRepository.Delete(userToDelete);
			await _unitOfWork.ProjectRepository.SaveChangesAsync();
			return Ok();
		}
    }
}
