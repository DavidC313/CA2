using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA2.Models
{
    public class Team
    {
        public Team()
        {
            Name = string.Empty;
            League = string.Empty;
            Country = string.Empty;
            Stadium = string.Empty;
            Manager = string.Empty;
            Players = new List<Player>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "League is required")]
        [StringLength(50, ErrorMessage = "League cannot exceed 50 characters")]
        public string League { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Founded year is required")]
        [Range(1800, 2024, ErrorMessage = "Founded year must be between 1800 and 2024")]
        public int FoundedYear { get; set; }

        [Required(ErrorMessage = "Stadium is required")]
        [StringLength(100, ErrorMessage = "Stadium name cannot exceed 100 characters")]
        public string Stadium { get; set; }

        [Required(ErrorMessage = "Manager is required")]
        [StringLength(100, ErrorMessage = "Manager name cannot exceed 100 characters")]
        public string Manager { get; set; }
        
        // Navigation property for players
        public virtual ICollection<Player> Players { get; set; }
    }
} 