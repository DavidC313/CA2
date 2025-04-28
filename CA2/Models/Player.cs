using System;

namespace CA2.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Position { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Appearances { get; set; }
        public required string Nationality { get; set; }
        
        // Foreign key
        public int TeamId { get; set; }
        // Navigation property
        public virtual Team? Team { get; set; }
    }
} 