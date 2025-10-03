using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var newUser = await context.Users
            .Include(x => x.Image)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return newUser;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Include(x=>x.Image)
            .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
    }

    public async Task AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> UpdateAsync(Guid userId, User updatedUser, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Image)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null || !user.IsActive)
            return null;

        user.UserName = updatedUser.UserName;
        user.FullName = updatedUser.FullName;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.PasswordHash = updatedUser.PasswordHash;
        user.UpdatedDate = DateTime.UtcNow;

        // работа с изображением
        if (updatedUser.Image != null)
        {
            if (user.Image == null)
            {
                // добавляем новое фото
                var newImage = new UserFile
                {
                    UserId = user.Id,
                    Name = updatedUser.Image.Name,
                    Url = updatedUser.Image.Url,
                    Extension = updatedUser.Image.Extension,
                    Size = updatedUser.Image.Size
                };

                context.UserFiles.Add(newImage);
                user.Image = newImage;
            }
            else
            {
                // обновляем существующее фото
                user.Image.Name = updatedUser.Image.Name;
                user.Image.Url = updatedUser.Image.Url;
                user.Image.Extension = updatedUser.Image.Extension;
                user.Image.Size = updatedUser.Image.Size;
            }
        }
        else if (user.Image != null && updatedUser.Image == null)
        {
            // удаляем фото, если в обновлении его нет
            context.UserFiles.Remove(user.Image);
            user.Image = null;
        }

        await context.SaveChangesAsync(cancellationToken);
        return user;
    }


    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive == true, cancellationToken);
        if (user == null)
            return false;

        user.IsActive = false;
        user.UpdatedDate = DateTime.UtcNow;

        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<string?> GetUserPicture(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Image)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user?.Image?.Url;
    }

    public async Task<bool> ExistsAsync(string username, string email, string phone, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.AnyAsync(
            u => (
                u.UserName == username
                || u.Email == email
                || u.PhoneNumber == phone
            ) && u.IsActive == true,
            cancellationToken
        );

        return user;
    }

}