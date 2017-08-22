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
    public class TasksController : BaseController
    {
		// GET: api/users        
		[HttpGet]
		public async Task<IEnumerable<ProjectTask>> Get()
		{
			return await _unitOfWork.ProjectTaskRepository.GetAllAsync();
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var projectTask = await _unitOfWork.ProjectTaskRepository.GetByIdAsync(id);
			if (projectTask == null)
			{
				return NotFound();
			}
			return Ok(projectTask);
		}

		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]ProjectTask value)
		{
			if (value == null)
			{
				return BadRequest();
			}

			await _unitOfWork.ProjectTaskRepository.AddAsync(value);
			_unitOfWork.ProjectTaskRepository.SaveChanges();

			return Ok();
			//return CreatedAtRoute("GetBook", new { id = createdBook.Id }, createdBook);
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody]ProjectTask value)
		{
			if (value == null)
			{
				return BadRequest();
			}
			var task = await _unitOfWork.ProjectTaskRepository.GetByIdAsync(id);

            task.Title = value.Title;
            task.Description = value.Description;
            task.Owner = value.Owner;
            task.OwnerId = value.OwnerId;
            task.Priority = value.Priority;
            task.Project = value.Project;
            task.ProjectId = value.ProjectId;
            task.DueDate = value.DueDate;
            task.StartDate = value.StartDate;
            task.EndDate = value.EndDate;
            task.Status = value.Status;

			_unitOfWork.ProjectTaskRepository.Update(task);
			_unitOfWork.ProjectTaskRepository.SaveChanges();
			return Ok(task);
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		//[ValidateUserExists]
		public async Task<IActionResult> Delete(int id)
		{
			var userToDelete = _unitOfWork.ProjectTaskRepository.GetById(id);
			_unitOfWork.ProjectTaskRepository.Delete(userToDelete);
			await _unitOfWork.ProjectTaskRepository.SaveChangesAsync();
			return Ok();
		}
	}
}
