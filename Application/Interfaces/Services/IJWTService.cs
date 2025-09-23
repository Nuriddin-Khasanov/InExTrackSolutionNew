using Domain.Models;

namespace Application.Interfaces.Services;

public interface IJWTService
{
    string GenerateToken(User user);
}
