using Application.Interfaces.Repositories;
using Domain.Enums;
using Domain.Models;
using Infrastructure.DataContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository(AppDBContext _context) : ICategoryRepository
{
    public async Task<bool> CategoryExistsAsync(string name, CategoryTypeEnum type, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(c => c.UserCategories)
            .Include(c => c.Image)
            .AnyAsync(c => c.Name == name && c.Type == type, cancellationToken);
    }

    public async Task<Category?> GetCategoryByNameAndTypeAsync(string name, CategoryTypeEnum type, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(c => c.UserCategories)
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Name == name && c.Type == type, cancellationToken);
    }

    public async Task<List<Category>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(c => c.Image)
            .Where(c => c.UserCategories.Any(uc => uc.UserId == userId && uc.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryById(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserCategories.Any(uc => uc.UserId == userId && uc.IsActive), cancellationToken);

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
        var category = await _context.Categories
            .Include(c => c.UserCategories)
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category == null)
            return null;

        updatedCategory.Adapt(category);
        category.Id = id;
        category.UpdatedDate = DateTime.UtcNow;

        // Если есть изображение
        if (updatedCategory.Image != null)
        {
            category.Image = updatedCategory.Image;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> DeleteCategory(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        // var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        //if (category == null)
        //    return false;
        //_context.Categories.Remove(category);

        var userCategory = await _context.UserCategories
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CategoryId == id, cancellationToken);

       if (userCategory == null || !userCategory.IsActive)
            return false;

       userCategory.IsActive = false;
       
       await _context.SaveChangesAsync(cancellationToken);

       return true;
    }

}
