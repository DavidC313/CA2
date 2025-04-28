using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA2.Models
{
    public class User
    {
        public User()
        {
            Username = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
            Role = "User";
            Token = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }
} 