using System;
namespace ZenLeap.Api.Models
{
    public class OwnersTeams
    {
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
