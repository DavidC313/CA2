using System;
using System.Collections.Generic;

namespace CA2.Models
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
        }

        public int TeamId { get; set; }
        public required string Name { get; set; }
        public required string League { get; set; }
        public required string Country { get; set; }
        public int FoundedYear { get; set; }
        public required string Stadium { get; set; }
        public required string Manager { get; set; }
        
        // Navigation property for players
        public virtual ICollection<Player> Players { get; set; }
    }
} 