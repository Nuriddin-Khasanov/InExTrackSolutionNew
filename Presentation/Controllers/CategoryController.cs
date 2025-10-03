using Application.DTOs.Requests;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[Authorize]
public class CategoryController(ICategoryService categoryService) : ApiBaseController
{
   
    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var categories = await categoryService.GetCategories(userId, cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await categoryService.GetCategoryById(id, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryRequestDto categoryDto, CancellationToken cancellationToken)
    {
        return Ok(await categoryService.CreateCategory(GetUserId(), categoryDto, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromForm] Guid id, CategoryRequestDto updatedCategory, CancellationToken cancellationToken)
    {
        return Ok(await categoryService.UpdateCategory(id, updatedCategory, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await categoryService.DeleteCategory(id, cancellationToken));
    }

}
