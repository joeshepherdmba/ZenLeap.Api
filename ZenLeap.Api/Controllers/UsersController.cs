using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenLeap.Api.Filters;
using ZenLeap.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// https://stormpath.com/blog/5-api-tips-dotnet-core Use this for Core
// Filters: https://msdn.microsoft.com/en-us/magazine/mt767699.aspx

namespace ZenLeap.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
		// GET: api/users
		// this maps to a get requests to:
		// domain/api/users
		// and domain/api/users?id=someid
		// and domain/api/users?mail=somemail
		// and domain/api/users?pw=somepw
		// and domain/api/users?mail=somemail&pw=somepw
		// and domain/api/users with any query string really
		[HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        //[ValidateUserExists]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User value)
        {
			if (value == null)
			{
				return BadRequest();
			}

			await _unitOfWork.UserRepository.AddAsync(value);
            _unitOfWork.UserRepository.SaveChanges();

            return Ok();
			//return CreatedAtRoute("GetBook", new { id = createdBook.Id }, createdBook);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
		//[ValidateUserExists]
        public async Task<IActionResult> Put(int id, [FromBody]User value)
        {
            if(value == null){
                return BadRequest();
            }
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            user.FirstName = value.FirstName;
            user.LastName = value.LastName;
            user.Email = value.Email;
            user.Projects = value.Projects;
            user.AssignedTasks = value.AssignedTasks;
            user.Password = value.Password;

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.UserRepository.SaveChanges();
			return Ok(user);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
		//[ValidateUserExists]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = _unitOfWork.UserRepository.GetById(id);
            _unitOfWork.UserRepository.Delete(userToDelete);
            await _unitOfWork.UserRepository.SaveChangesAsync();
			return Ok();
        }
    }
}
