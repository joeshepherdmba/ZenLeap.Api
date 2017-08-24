using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ZenLeap.Api.Models
{
    public class User : IdentityUser<int> // TODO: wire up Identity
    {

		public string LastName { get; set; }

		public string FirstName { get; set; } 
        //[Required]
        //public string Email { get; set; }

        public string Password { get; set; } //TODO: Password hash

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ProjectTask> AssignedTasks { get; set; }

		///// <summary>
		///// Navigation property for the roles this user belongs to.
		///// </summary>
		//public virtual ICollection<TUserRole> Roles { get; } = new List<TUserRole>();

		///// <summary>
		///// Navigation property for the claims this user possesses.
		///// </summary>
		//public virtual ICollection<TUserClaim> Claims { get; } = new List<TUserClaim>();

		///// <summary>
		///// Navigation property for this users login accounts.
		///// </summary>
		//public virtual ICollection<TUserLogin> Logins { get; } = new List<TUserLogin>();
	}
}