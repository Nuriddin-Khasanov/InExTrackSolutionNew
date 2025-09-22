using System.ComponentModel.DataAnnotations;

namespace InExTrack.Application.DTOs.Requests
{
    public class RegisterRequest_
    {
        [Required]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать минимум 3 символа")]
        public required string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public required string Password { get; set; }
    }
}
