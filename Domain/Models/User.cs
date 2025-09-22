using System.ComponentModel.DataAnnotations;
using InExTrack.Domain.Commons;

namespace InExTrack.Domain.Models
{
    public class User: Entity
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Required]
        public string? FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        [Required]
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public UserFile? Image { get; set; }
        public required List<UserCategory> UserCategories { get; set; }
    }
}
