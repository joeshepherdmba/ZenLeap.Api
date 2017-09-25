using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
    public class Event
    {
		[Key]
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		public string ProjectOwnerId { get; set; }

		[ForeignKey("EventOwnerId")]
		public virtual User EventOwner { get; set; }

		[Required]
		public int TeamId { get; set; }

		[ForeignKey("TeamId")]
		public Team Team { get; set; }

	}
}
