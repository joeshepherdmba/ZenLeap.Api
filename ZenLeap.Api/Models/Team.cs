using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
    public class Team
    {
		[Key]
		public int Id { get; set; }

		[Required]
		public string TeamName { get; set; }

		[DataType(DataType.Date)]
		public DateTime DateEstablished { get; set; }

		public string OwnerId { get; set; }

		[ForeignKey("OwnerId")]
		public virtual User Owner { get; set; }

		public virtual ICollection<TeamMembers> Members { get; set; }

	}
}
