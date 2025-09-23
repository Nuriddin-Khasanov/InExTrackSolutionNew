using Application.DTOs.Responses;

namespace Application.DTOs
{
    public class AuthResultDto
    {
        public UserResponseDto UserResponseDto { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
