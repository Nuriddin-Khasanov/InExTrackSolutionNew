using InExTrack.Application.DTOs;
using InExTrack.Application.DTOs.Responses;

namespace InExTrack.Application.Interfaces.Services
{
    public interface IUserCategoryService
    {
        Task<ApiResponse<IEnumerable<UserCategoryDto>>> GetUserCategoriesAsync(CancellationToken cancellationToken = default);
        Task<ApiResponse<UserCategoryDto>> GetUserCategoryByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ApiResponse<UserCategoryDto>> AddUserCategoryAsync(UserCategoryDto userCategoryDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<UserCategoryDto>> UpdateUserCategoryAsync(Guid id, UserCategoryDto userCategoryDto, CancellationToken cancellationToken = default);
        Task<ApiResponse<bool>> DeleteUserCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
