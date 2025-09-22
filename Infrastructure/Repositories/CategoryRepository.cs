using InExTrack.Application.Interfaces.Repositories;
using InExTrack.Domain.Enums;
using InExTrack.Domain.Models;
using InExTrack.Infrastructure.DataContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InExTrack.Infrastructure.Repositories
{
    public class CategoryRepository(AppDBContext _context) : ICategoryRepository
    {
        public async Task<bool> CategoryExistsAsync(string name, CategoryTypeEnum type, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && c.Type == type, cancellationToken);
        }

        public async Task<Category?> GetCategoryByNameAndTypeAsync(string name, CategoryTypeEnum type, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name && c.Type == type, cancellationToken);
        }
        public async Task<List<Category>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(c => c.UserCategories.Any(uc => uc.UserId == userId && uc.IsActive))
                .ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetCategoryById(Guid userId, Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return category;
        }

        public async Task<Category> CreateCategory(Guid userId, Category category, CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return category;
        }

        public async Task<Category?> UpdateCategory(Guid userId, Guid id, Category updatedCategory, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
                return null;

            updatedCategory.Adapt(category);
            category.Id = id;

            //// Если есть изображение
            //if (updatedCategory.Image != null)
            //{
            //    category.Image = updatedCategory.Image;
            //}

            await _context.SaveChangesAsync(cancellationToken);
            return category;
        }

        public async Task<bool> DeleteCategory(Guid userId, Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
                return false;
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
