using System;
using static ZenLeap.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
	public class TimeEntry
	{
        public int Id { get; set; }
        public string Notes { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int Minutes { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("Approver")]
        public int ApproverID { get; set; }
        public virtual User Approver { get; set; }
        public TimeStatus Status { get; set; }

        [ForeignKey("ProejctTask")]
        public int TaskID { get; set; }
        public virtual ProjectTask ProejctTask { get; set; }
    }
}