using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
    public class Activity
    {
		[Key]
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

        public string OwnerId { get; set; }

		[ForeignKey("OwnerId")]
		public virtual User Owner { get; set; }

		[Required]
		public int TeamId { get; set; }

		[ForeignKey("TeamId")]
		public Team Team { get; set; }

	}
}
