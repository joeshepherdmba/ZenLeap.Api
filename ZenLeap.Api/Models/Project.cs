using System;
using System.Collections.Generic;
using ZenLeap.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public double ProjectValue { get; set; }
        public double VelocityFactor { get; set; }
        public double HealthFactor { get; set; }

		public int ProjectOwnerId { get; set; }

        [ForeignKey("ProjectOwnerId")]
        public virtual User ProjectOwner { get; set; }
        [Required]
        public int CompanyId { get; set; }
		
        [ForeignKey("CompanyId")]
		public Company Company { get; set; }

        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }
    }
}
