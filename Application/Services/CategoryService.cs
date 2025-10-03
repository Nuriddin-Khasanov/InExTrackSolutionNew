using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Models;
using Mapster;

namespace Application.Services;

public class CategoryService(
        ICategoryRepository categoryRepository,
        IFileService fileService
    ) : ICategoryService
{
    public async Task<ApiResponse<IEnumerable<CategoryResponceDto>>> GetCategories(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            var categories = await categoryRepository.GetCategoriesAsync(userId, cancellationToken);

            if (categories == null || categories.Count == 0)
                return new ApiResponse<IEnumerable<CategoryResponceDto>>(404, "Категории не найдены");

            var categoriesDto = categories.Adapt<IEnumerable<CategoryResponceDto>>();

            return new ApiResponse<IEnumerable<CategoryResponceDto>>(200, categoriesDto, "Категории успешно получены!");
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<CategoryResponceDto>>(500, null, $"Ошибка при получении категорий: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoryResponceDto?>> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            if (id == Guid.Empty)
                return new ApiResponse<CategoryResponceDto?>(400, "Id не может быть пустым!");

            var category = await categoryRepository.GetCategoryById(id, cancellationToken);
            if (category == null)
                return new ApiResponse<CategoryResponceDto?>(404, "Категория не найдена");

            var categoryDto = category.Adapt<CategoryResponceDto>();

            return new ApiResponse<CategoryResponceDto?>(200, categoryDto, "Категория успешно получена!");
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponceDto?>(500, $"Ошибка при получении категории: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoryResponceDto>> CreateCategory(Guid userId, CategoryRequestDto categoryDto, CancellationToken cancellationToken)
    {
        try
        {
            if (categoryDto == null)
                return new ApiResponse<CategoryResponceDto>(400, "Данные категории не предоставлены!");

            // Проверяем, существует ли уже такая категория
            var existingCategory = await categoryRepository.GetCategoryByNameAndTypeAsync
            (
                userId,
                categoryDto.Name ?? "", 
                categoryDto.Type, 
                cancellationToken
            );

            // Если категория существует, проверяем связь с пользователем
            if (existingCategory != null)
            {
                if(existingCategory.IsActive)
                    return new ApiResponse<CategoryResponceDto>(400, null, "Категория с таким именем и типом уже существует");

                var newCategory = await categoryRepository.CreateCategory(existingCategory.Id, cancellationToken);

                if (!newCategory)
                    return new ApiResponse<CategoryResponceDto>(500, "Ошибка при создании категории");

                var resultDto = existingCategory.Adapt<CategoryResponceDto>();
                return new ApiResponse<CategoryResponceDto>(200, resultDto, "Категория успешно добавлена пользователю!");
            }

            // Создаем новую категорию
            var category = categoryDto.Adapt<Category>();
            category.UserId = userId;

            if (categoryDto.ImageURL != null)
            {
                var savedFile = await fileService.SaveAsync(categoryDto.ImageURL);
                if (savedFile == null)
                    return new ApiResponse<CategoryResponceDto>(400, "Ошибка при сохранении файла изображения пользователя.");
                category.Image = new CategoryFile()
                {
                    CategoryId = category.Id,
                    Name = savedFile.Name,
                    Url = savedFile.Url,
                    Size = savedFile.Size,
                    Extension = savedFile.Extension
                };
            }

            var createdCategory = await categoryRepository.CreateCategory(category, cancellationToken);
            if (createdCategory == null)
                return new ApiResponse<CategoryResponceDto>(500, null, "Ошибка при создании категории");

            var createdDto = createdCategory.Adapt<CategoryResponceDto>();
            return new ApiResponse<CategoryResponceDto>(201, createdDto, "Категория успешно создана!");
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponceDto>(500, null, $"Ошибка при создании категории: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CategoryResponceDto?>> UpdateCategory(Guid userId, Guid id, CategoryRequestDto categoryDto, CancellationToken cancellationToken)
    {
        try
        {
            if (id == Guid.Empty)
                return new ApiResponse<CategoryResponceDto?>(400, null, "Id не может быть пустым!");

            // Получаем существующую категорию
            var existingCategory = await categoryRepository.GetCategoryById(id, cancellationToken);

            if (existingCategory == null)
                return new ApiResponse<CategoryResponceDto?>(404, "Категория не найдена");

            if (existingCategory.Name == categoryDto.Name 
                && existingCategory.Type == categoryDto.Type 
                && existingCategory.Description == categoryDto.Description
                && ((existingCategory.Image?.Url ?? string.Empty) == (categoryDto.ImageURL?.ToString() ?? string.Empty)))
                return new ApiResponse<CategoryResponceDto?>(400, "Вы не изменили категории!");

            // Проверка на дубликаты
            var duplicateExists = await categoryRepository.CategoryExistsAsync(
                userId, categoryDto, cancellationToken);

            if (duplicateExists)
                return new ApiResponse<CategoryResponceDto?>(400, "Категория с таким именем и типом уже существует");

            // Частичное обновление с помощью AutoMapper
            var categoryAdapt = categoryDto.Adapt<Category>();
            categoryAdapt.Id = id;

            var categoryPicture = await categoryRepository.GetCategoryPicture(id, cancellationToken);
            if(!string.IsNullOrWhiteSpace(categoryPicture))
                await fileService.RemoveAsync(categoryPicture);

            if (categoryDto.ImageURL != null)
            {
                var savedFile = await fileService.SaveAsync(categoryDto.ImageURL);
                if (savedFile == null)
                    return new ApiResponse<CategoryResponceDto?>(400, "Ошибка при сохранении файла изображения категории.");
                categoryAdapt.Image = new CategoryFile()
                {
                    CategoryId = categoryAdapt.Id,
                    Name = savedFile.Name,
                    Url = savedFile.Url,
                    Size = savedFile.Size,
                    Extension = savedFile.Extension
                };
            }

            var updatedCategory = await categoryRepository.UpdateCategory(id, categoryAdapt, cancellationToken);

            var resultDto = updatedCategory.Adapt<CategoryResponceDto>();

            return new ApiResponse<CategoryResponceDto?>(200, resultDto, "Категория успешно обновлена!");
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponceDto?>(500, $"Ошибка при обновлении категории: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            if (id == Guid.Empty)
                return new ApiResponse<bool>(400, "Id не может быть пустым!");

            // Soft delete - деактивируем связь пользователя с категорией
            var result = await categoryRepository.DeleteCategory(id, cancellationToken);

            if (!result)
                return new ApiResponse<bool>(404, "Связь с категорией не найдена");

            return new ApiResponse<bool>(200, true, "Категория успешно удалена у пользователя!");
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>(500, $"Ошибка при удалении категории: {ex.Message}");
        }
    }
}
