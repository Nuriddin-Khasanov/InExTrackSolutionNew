using System.ComponentModel.DataAnnotations;

namespace InExTrack.Application.DTOs.Requests
{
    public class LoginRequest_
    {
        [Required]  // Атрибут объязательное поле
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
