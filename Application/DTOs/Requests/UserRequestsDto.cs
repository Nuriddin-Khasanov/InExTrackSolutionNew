using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace InExTrack.Application.DTOs.Requests
{
    public class UserRequestsDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать минимум 3 символа")]
        public required string UserName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public required string PasswordHash { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать минимум 3 символа")]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        public IFormFile? ImageURL { get; set; }
    }
}
