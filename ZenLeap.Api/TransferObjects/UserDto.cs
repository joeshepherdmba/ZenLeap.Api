using System;
using System.Collections.Generic;

namespace ZenLeap.Api.TransferObjects
{
    public class UserDto
    {
        public UserDto()
        {
        }

		public int Id { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public virtual ICollection<ProjectDto> Projects { get; set; }
		public virtual ICollection<ProjectTaskDto> AssignedTasks { get; set; }
	}
}
