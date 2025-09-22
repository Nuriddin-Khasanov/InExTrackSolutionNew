using InExTrack.Domain.Enums;
using InExTrack.Domain.Models;

namespace InExTrack.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<bool> CategoryExistsAsync(string name, CategoryTypeEnum type, CancellationToken cancellationToken = default);
        public Task<Category?> GetCategoryByNameAndTypeAsync(string name, CategoryTypeEnum type, CancellationToken cancellationToken = default);
        public Task<List<Category>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default);
        public Task<Category?> GetCategoryById(Guid userId, Guid id, CancellationToken cancellationToken);
        public Task<Category> CreateCategory(Guid userId, Category category, CancellationToken cancellationToken);
        public Task<Category?> UpdateCategory(Guid userId, Guid id, Category updatedCategory, CancellationToken cancellationToken);
        public Task<bool> DeleteCategory(Guid userId, Guid id, CancellationToken cancellationToken);
    }
}
