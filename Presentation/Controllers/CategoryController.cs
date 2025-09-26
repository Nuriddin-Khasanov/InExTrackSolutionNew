using Application.DTOs.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[Authorize]
public class CategoryController(ICategoryService _categoryService) : ApiBaseController
{
   
    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var categories = await _categoryService.GetCategories(userId, cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.GetCategoryById(GetUserId(), id, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryRequestDto categoryDto, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.CreateCategory(GetUserId(), categoryDto, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromForm] Guid id, CategoryRequestDto updatedCategory, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.UpdateCategory(GetUserId(), id, updatedCategory, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.DeleteCategory(GetUserId(), id, cancellationToken));
    }

}
