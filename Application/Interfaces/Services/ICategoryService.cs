using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    public Task<ApiResponse<IEnumerable<CategoryResponceDto>>> GetCategories(Guid userId, CancellationToken cancellationToken);
    public Task<ApiResponse<CategoryResponceDto?>> GetCategoryById(Guid id, CancellationToken cancellationToken);
    public Task<ApiResponse<CategoryResponceDto>> CreateCategory(Guid userId, CategoryRequestDto categoryDto, CancellationToken cancellationToken);
    public Task<ApiResponse<CategoryResponceDto?>> UpdateCategory(Guid userId, Guid id, CategoryRequestDto categoryDto, CancellationToken cancellationToken);
    public Task<ApiResponse<bool>> DeleteCategory(Guid id, CancellationToken cancellationToken);
}
