using System;
namespace ZenLeap.Api.Models
{
    public class UserTeams
    {
        public string UserId { get; set; } 
        public User User { get; set; } 

        public string TeamId { get; set; } 
        public Team Team { get; set; }
    }
}
