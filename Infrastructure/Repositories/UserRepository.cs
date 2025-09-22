using InExTrack.Application.DTOs.Requests;
using InExTrack.Application.Interfaces.Repositories;
using InExTrack.Domain.Models;
using InExTrack.Infrastructure.DataContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InExTrack.Infrastructure.Repositories;

public class UserRepository(AppDBContext _context) : IUserRepository
{

    //public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    //{
    //    return await _context.Users.ToListAsync(cancellationToken);
    //}

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return newUser;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> UpdateAsync(Guid userId, UserRequestsDto userRequestsDto, CancellationToken cancellationToken = default)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return null; 

        userRequestsDto.Adapt(user);
        user.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive == true, cancellationToken);
        if (user == null)
            return false;

        user.IsActive = false;
        user.UpdatedDate = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(string username, string email, string phone, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.AnyAsync(
            u => (
                u.UserName == username 
                || u.Email == email
                || u.PhoneNumber == phone
            )   && u.IsActive == true, 
            cancellationToken
        );

        if (user)
            return true;

        return false;
    }

}