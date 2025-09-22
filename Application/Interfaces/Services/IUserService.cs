using InExTrack.Application.DTOs;
using InExTrack.Application.DTOs.Requests;
using InExTrack.Application.DTOs.Responses;

namespace InExTrack.Application.Interfaces.Services;

public interface IUserService
{
   // public Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAll(CancellationToken cancellationToken);
    public Task<ApiResponse<UserResponseDto>> GetUserById(Guid _userId, CancellationToken cancellationToken);
    public Task<ApiResponse<UserResponseDto>> UpdateUserById(Guid _userId, UserRequestsDto userRequestsDto, CancellationToken cancellationToken);
    public Task<ApiResponse<bool>> RegisterUserAsync(UserRequestsDto _user, CancellationToken cancellationToken);
    public Task<ApiResponse<bool>> DeleteUser(Guid id, CancellationToken cancellationToken);

    Task<ApiResponse<AuthResultDto>> AuthenticateAsync(string username, string password);

}
