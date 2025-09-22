using InExTrack.Domain.Models;

namespace InExTrack.Application.Interfaces.Services
{
    public interface IJWTService
    {
        string GenerateToken(User user);
    }
}
