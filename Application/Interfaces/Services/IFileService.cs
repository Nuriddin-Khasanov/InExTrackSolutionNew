using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IFileService
{
    Task RemoveAsync(string fileName);
    Task<FileDto> SaveAsync(IFormFile file);
}
