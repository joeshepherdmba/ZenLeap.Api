using System;
using System.Collections.Generic;
using static ZenLeap.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }       
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime EndDate { get; set; }

		/* TODO: public static bool IsWorkingDay(this DateTime date)
			    {
			        return date.DayOfWeek != DayOfWeek.Saturday
			            && date.DayOfWeek != DayOfWeek.Sunday;
			    
		        public int EstDays { get; }
		        public int EstHours { get; }
        }*/

		public int ProjectId { get; set; }
        [Required]
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
        //public virtual ICollection<TimeEntry> TimeEntry { get; set; }
    }
}
