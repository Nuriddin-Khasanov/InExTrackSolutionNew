using Application.DTOs;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IUserCategoryRepository
    {
        Task<UserCategory?> GetUserCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default);
        Task<UserCategory> AddOrActivateUserCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserCategory>> GetUserCategoriesAsync(CancellationToken cancellationToken = default);
        Task<UserCategory> GetUserCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserCategory> AddUserCategoryAsync(UserCategory userCategory, CancellationToken cancellationToken = default);
        Task<UserCategory> UpdateUserCategoryAsync(Guid id, UserCategoryDto userCategoryDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserCategoryAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserCategory?> HasUserCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default);
    }
}
