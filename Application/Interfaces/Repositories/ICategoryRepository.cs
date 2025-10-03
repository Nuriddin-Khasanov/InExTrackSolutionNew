using Application.DTOs.Requests;
using Domain.Enums;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        public Task<bool> CategoryExistsAsync(Guid userId, CategoryRequestDto categoryDto, CancellationToken cancellationToken = default);
        public Task<Category?> GetCategoryByNameAndTypeAsync(Guid userId, string name, CategoryTypeEnum type, CancellationToken cancellationToken = default);
        public Task<List<Category>> GetCategoriesAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Category?> GetCategoryById(Guid id, CancellationToken cancellationToken);
        public Task<Category> CreateCategory(Category category, CancellationToken cancellationToken);
        public Task<bool> CreateCategory(Guid id, CancellationToken cancellationToken);
        public Task<Category?> UpdateCategory(Guid id, Category updatedCategory, CancellationToken cancellationToken);
        public Task<bool> DeleteCategory(Guid id, CancellationToken cancellationToken);
        public Task<string?> GetCategoryPicture(Guid categoryId, CancellationToken cancellationToken = default);
    }
}
