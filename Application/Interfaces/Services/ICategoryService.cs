using Application.DTOs;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    public Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategories(Guid userId, CancellationToken cancellationToken);
    public Task<ApiResponse<CategoryDto?>> GetCategoryById(Guid userId, Guid id, CancellationToken cancellationToken);
    public Task<ApiResponse<CategoryDto>> CreateCategory(Guid userId, CategoryDto categoryDto, CancellationToken cancellationToken);
    public Task<ApiResponse<CategoryDto?>> UpdateCategory(Guid userId, Guid id, CategoryDto categoryDto, CancellationToken cancellationToken);
    public Task<ApiResponse<bool>> DeleteCategory(Guid userId, Guid id, CancellationToken cancellationToken);
}
