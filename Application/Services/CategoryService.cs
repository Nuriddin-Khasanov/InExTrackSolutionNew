using InExTrack.Application.DTOs;
using InExTrack.Application.DTOs.Responses;
using InExTrack.Application.Interfaces.Repositories;
using InExTrack.Application.Interfaces.Services;
using InExTrack.Domain.Models;
using Mapster;
using MapsterMapper;

namespace InExTrack.Application.Services
{
    public class CategoryService(ICategoryRepository _categoryRepository, IUserCategoryRepository _userCategoryRepository) : ICategoryService
    {
        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategories(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryRepository.GetUserCategoriesAsync(userId, cancellationToken);
                var categoriesDto = categories.Adapt<IEnumerable<CategoryDto>>();

                return new ApiResponse<IEnumerable<CategoryDto>>(200, categoriesDto, "Категории успешно получены!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CategoryDto>>(500, null, $"Ошибка при получении категорий: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryDto?>> GetCategoryById(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    return new ApiResponse<CategoryDto?>(400, "Id не может быть пустым!");

                // Проверка доступа пользователя к категории
                var userCategory = await _userCategoryRepository.HasUserCategoryAsync(userId, id, cancellationToken);
                if (userCategory == null)
                    return new ApiResponse<CategoryDto?>(403, "Доступ к категории запрещен");

                var category = await _categoryRepository.GetCategoryById(userId, id, cancellationToken);
                if (category == null)
                    return new ApiResponse<CategoryDto?>(404, "Категория не найдена");

                var categoryDto = category.Adapt<CategoryDto>();

                return new ApiResponse<CategoryDto?>(200, categoryDto, "Категория успешно получена!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto?>(500, $"Ошибка при получении категории: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryDto>> CreateCategory(Guid userId, CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            try
            {
                // Проверяем, существует ли уже такая категория
                var existingCategory = await _categoryRepository.GetCategoryByNameAndTypeAsync(
                    categoryDto.Name ?? "", categoryDto.Type, cancellationToken);

                // Если категория существует, проверяем связь с пользователем
                if (existingCategory != null)
                {
                    var userCategory = await _userCategoryRepository.GetUserCategoryAsync(userId, existingCategory.Id, cancellationToken);

                    if (userCategory != null && userCategory.IsActive)
                    {
                        return new ApiResponse<CategoryDto>(400, null, "Категория уже существует у пользователя");
                    }

                    // Активируем или создаем связь
                    await _userCategoryRepository.AddOrActivateUserCategoryAsync(userId, existingCategory.Id, cancellationToken);

                    var resultDto = existingCategory.Adapt<CategoryDto>();
                    return new ApiResponse<CategoryDto>(200, resultDto, "Категория успешно добавлена пользователю!");
                }

                // Создаем новую категорию
                var category = categoryDto.Adapt<Category>();
                var createdCategory = await _categoryRepository.CreateCategory(userId, category, cancellationToken);

                // Связываем категорию с пользователем
                await _userCategoryRepository.AddOrActivateUserCategoryAsync(userId, createdCategory.Id, cancellationToken);

                var createdDto = createdCategory.Adapt<CategoryDto>();
                return new ApiResponse<CategoryDto>(201, createdDto, "Категория успешно создана!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto>(500, null, $"Ошибка при создании категории: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryDto?>> UpdateCategory(Guid userId, Guid id, CategoryDto categoryDto, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    return new ApiResponse<CategoryDto?>(400, null, "Id не может быть пустым!");

                // Получаем существующую категорию
                var existingCategory = await _categoryRepository.GetCategoryById(userId, id, cancellationToken);
                if (existingCategory == null)
                    return new ApiResponse<CategoryDto?>(404, null, "Категория не найдена");

                // Проверяем права доступа
                var userCategory = await _userCategoryRepository.GetUserCategoryAsync(userId, id, cancellationToken);
                if (userCategory == null || !userCategory.IsActive)
                    return new ApiResponse<CategoryDto?>(403, null, "Доступ к категории запрещен");

                // Проверка на дубликаты
                if (existingCategory.Name != categoryDto.Name || existingCategory.Type != categoryDto.Type)
                {
                    var duplicateExists = await _categoryRepository.CategoryExistsAsync(
                        categoryDto.Name ?? "", categoryDto.Type, cancellationToken);

                    if (duplicateExists)
                    {
                        return new ApiResponse<CategoryDto?>(400, null, "Категория с таким именем и типом уже существует");
                    }
                }

                // Частичное обновление с помощью AutoMapper
                categoryDto.Adapt(existingCategory);
                existingCategory.UpdatedDate = DateTime.UtcNow;

                var updatedCategory = await _categoryRepository.UpdateCategory(userId, id, existingCategory, cancellationToken);

                var resultDto = updatedCategory.Adapt<CategoryDto>();
                return new ApiResponse<CategoryDto?>(200, resultDto, "Категория успешно обновлена!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryDto?>(500, null, $"Ошибка при обновлении категории: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteCategory(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == Guid.Empty)
                    return new ApiResponse<bool>(400, false, "Id не может быть пустым!");

                // Проверяем права доступа пользователя к категории
                var userCategory = await _userCategoryRepository.GetUserCategoryAsync(userId, id, cancellationToken);
                if (userCategory == null || !userCategory.IsActive)
                    return new ApiResponse<bool>(403, false, "Доступ к категории запрещен или категория уже удалена");

                // Soft delete - деактивируем связь пользователя с категорией
                var result = await _userCategoryRepository.DeleteUserCategoryAsync(userCategory.Id, cancellationToken);

                if (!result)
                    return new ApiResponse<bool>(404, false, "Связь с категорией не найдена");

                return new ApiResponse<bool>(200, true, "Категория успешно удалена у пользователя!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500, false, $"Ошибка при удалении категории: {ex.Message}");
            }
        }
    }
}
