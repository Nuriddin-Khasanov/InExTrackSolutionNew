using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserCategoryRepository(AppDBContext _context) : IUserCategoryRepository
{
    public async Task<UserCategory?> GetUserCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.UserCategories
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CategoryId == categoryId, cancellationToken);
    }

    public async Task<UserCategory> AddOrActivateUserCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var existingUserCategory = await GetUserCategoryAsync(userId, categoryId, cancellationToken);

        if (existingUserCategory != null)
        {
            if (!existingUserCategory.IsActive)
            {
                existingUserCategory.IsActive = true;
                _context.UserCategories.Update(existingUserCategory);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return existingUserCategory;
        }

        var newUserCategory = new UserCategory
        {
            UserId = userId,
            CategoryId = categoryId,
            IsActive = true
        };

        await _context.UserCategories.AddAsync(newUserCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newUserCategory;
    }


    public async Task<IEnumerable<UserCategory>> GetUserCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var userCategories = await _context.UserCategories.Where(uc => uc.IsActive).ToListAsync(cancellationToken);

        return userCategories;
    }

    public async Task<UserCategory> GetUserCategoryByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userCategory = await _context.UserCategories.FirstOrDefaultAsync(uc => uc.Id == userId, cancellationToken);

        return userCategory ?? throw new InvalidOperationException($"UserCategory с UserId '{userId}' не найден.");
    }



    public async Task<UserCategory> AddUserCategoryAsync(UserCategory userCategoryDto, CancellationToken cancellationToken = default)
    {
        await  _context.UserCategories.AddAsync(userCategoryDto, cancellationToken);
        await  _context.SaveChangesAsync(cancellationToken);

        return userCategoryDto;
    }

    public async Task<UserCategory> UpdateUserCategoryAsync(Guid id, UserCategoryDto userCategoryDto, CancellationToken cancellationToken = default)
    {
        var existingUserCategory = await _context.UserCategories.Where(uc => uc.IsActive).FirstOrDefaultAsync(uc => uc.Id == id, cancellationToken);
        if (existingUserCategory == null)
            throw new InvalidOperationException($"UserCategory with ID '{id}' not found.");
    
        existingUserCategory.UserId = userCategoryDto.UserId;
        existingUserCategory.CategoryId = userCategoryDto.CategoryId;
        _context.UserCategories.Update(existingUserCategory);
        await _context.SaveChangesAsync(cancellationToken);

        return existingUserCategory;
    }

    public async Task<bool> DeleteUserCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userCategory = await _context.UserCategories.Where(uc => uc.IsActive).FirstOrDefaultAsync(uc => uc.Id == id, cancellationToken);
        if (userCategory != null)
        {
            userCategory.IsActive = false;
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result;
        }

        throw new InvalidOperationException($"UserCategory with ID '{id}' not found.");
    }

    public async Task<UserCategory?> HasUserCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        var result = await _context.UserCategories.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CategoryId == categoryId, cancellationToken);
    
        if(result == null)
            return null;
        if (!result.IsActive)
        {
            result.IsActive = true;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}
