using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface IUserService
{
   // public Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAll(CancellationToken cancellationToken);
    public Task<ApiResponse<UserResponseDto>> GetUserById(Guid userId, CancellationToken cancellationToken);
    public Task<ApiResponse<UserResponseDto>> UpdateUserById(Guid userId, UserRequestsDto userRequestsDto, CancellationToken cancellationToken);
    public Task<ApiResponse<bool>> RegisterUserAsync(UserRequestsDto user, CancellationToken cancellationToken);
    public Task<ApiResponse<bool>> DeleteUser(Guid id, CancellationToken cancellationToken);

    public Task<ApiResponse<AuthResultDto>> AuthenticateAsync(string username, string password);

}
