using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA2.Models
{
    public class Player
    {
        public Player()
        {
            Name = string.Empty;
            Position = string.Empty;
            Nationality = string.Empty;
        }

        [Key]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(16, 50, ErrorMessage = "Age must be between 16 and 50")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(20, ErrorMessage = "Position cannot exceed 20 characters")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Goals are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Goals cannot be negative")]
        public int Goals { get; set; }

        [Required(ErrorMessage = "Assists are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Assists cannot be negative")]
        public int Assists { get; set; }

        [Required(ErrorMessage = "Appearances are required")]
        [Range(0, int.MaxValue, ErrorMessage = "Appearances cannot be negative")]
        public int Appearances { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Team ID is required")]
        public int TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team? Team { get; set; }
    }
} 