using Application.DTOs.Requests;
using Domain.Models;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
   // Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
   
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task AddAsync(User user);
    Task<User?> UpdateAsync(Guid _userId, UserRequestsDto userRequestsDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string username, string email, string phone, CancellationToken cancellationToken = default);

}
