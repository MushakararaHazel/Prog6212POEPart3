using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class AppUser
    {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;   

        [Required, MaxLength(200)]
        public string PasswordHash { get; set; } = string.Empty; 

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Range(0, 2000)]
        public decimal HourlyRate { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
