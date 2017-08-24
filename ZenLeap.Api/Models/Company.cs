using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenLeap.Api.Models
{
    public class Company{
        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateEstablished { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }

        public virtual ICollection<User> Employees { get; set; }
        public virtual ICollection<Project> Projects { get; set; } 
    }    
}
