using System;
namespace ZenLeap.Api.Models
{
    public class UserTeams
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public string MemberId { get; set; }
        public User Member { get; set; }
    }
}
