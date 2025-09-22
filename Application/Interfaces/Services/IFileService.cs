using InExTrack.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace InExTrack.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task RemoveAsync(string fileName);
        Task<FileDto> SaveAsync(IFormFile file);
    }
}
