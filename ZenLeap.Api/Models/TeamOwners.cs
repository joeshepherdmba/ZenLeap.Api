using System;
namespace ZenLeap.Api.Models
{
    public class TeamOwners
    {
        public string TeamId { get; set; }
        public Team Team { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
