using Application.DTOs.Requests;
using Application.Interfaces.Repositories;
using Domain.Enums;
using Domain.Models;
using Infrastructure.DataContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<bool> CategoryExistsAsync(Guid userId, CategoryRequestDto categoryDto, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .Include(c => c.Image)
            .AnyAsync(
                c => c.UserId == userId 
                && c.Name == categoryDto.Name 
                && c.Type == categoryDto.Type 
                && c.Description == categoryDto.Description, 
                cancellationToken);
    }

    public async Task<Category?> GetCategoryByNameAndTypeAsync(Guid userId, string name, CategoryTypeEnum type, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Name == name && c.Type == type && c.UserId == userId, cancellationToken);
    }

    public async Task<List<Category>> GetCategoriesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Categories
            .Include(c => c.Image)
            .Where(c => c.UserId == userId && c.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryById(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive, cancellationToken);

        return category;
    }

    public async Task<Category> CreateCategory(Category category, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return category;
    }

    public async Task<bool> CreateCategory(Guid id, CancellationToken cancellationToken)
    {
        var newCategory = await context.Categories.Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken) 
            ?? throw new InvalidOperationException($"Category with id {id} not found.");
        
        newCategory.IsActive = true;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Category?> UpdateCategory(Guid id, Category updatedCategory, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (category == null || !category.IsActive)
            return null;

        category.Id = id;
       // category.UserId = updatedCategory.UserId;
        category.Name = updatedCategory.Name;
        category.Type = updatedCategory.Type;
        category.Description = updatedCategory.Description;
        category.UpdatedDate = DateTime.UtcNow;

        // Если есть изображение
        if (updatedCategory.Image != null)
        {
            if (category.Image == null)
            {
                var newImage = new CategoryFile 
                { 
                    CategoryId = category.Id,
                    Name = updatedCategory.Image.Name,
                    Url = updatedCategory.Image.Url,
                    Extension = updatedCategory.Image.Extension,
                    Size = updatedCategory.Image.Size
                };

                context.CategotyFiles.Add(newImage);
                category.Image = newImage;
            }
            else
            {
                category.Image.Name = updatedCategory.Image.Name;
                category.Image.Url = updatedCategory.Image.Url;
                category.Image.Extension = updatedCategory.Image.Extension;
                category.Image.Size = updatedCategory.Image.Size;
            }
        }
        else if(category.Image != null && updatedCategory.Image == null)
        {
            context.CategotyFiles.Remove(category.Image);
            category.Image = null;
        } 

        await context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> DeleteCategory(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

       if (category == null || !category.IsActive)
            return false;

        category.IsActive = false;
       
       await context.SaveChangesAsync(cancellationToken);

       return true;
    }

    public async Task<string?> GetCategoryPicture(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await context.Categories
            .Include(c => c.Image)
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

        return category?.Image?.Url;
    }
}
