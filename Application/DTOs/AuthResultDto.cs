using InExTrack.Application.DTOs.Responses;

namespace InExTrack.Application.DTOs
{
    public class AuthResultDto
    {
        public UserResponseDto UserResponseDto { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
