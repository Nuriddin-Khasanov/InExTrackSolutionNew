using Application.DTOs;
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
        var categories = await _categoryService.GetCategories(GetUserId(), cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.GetCategoryById(GetUserId(), id, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.CreateCategory(GetUserId(), categoryDto, cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory([FromBody] Guid id, CategoryDto updatedCategory, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.UpdateCategory(GetUserId(), id, updatedCategory, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.DeleteCategory(GetUserId(), id, cancellationToken));
    }

}
