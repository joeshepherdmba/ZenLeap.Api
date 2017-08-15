using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZenLeap.Api.Models
{
	public class User
	{
        [Key]
		public int Id { get; set; }
        [Required]
		public string LastName { get; set; }
        [Required]
		public string FirstName { get; set; } 
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ProjectTask> AssignedTasks { get; set; }
	}
}