using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Requests
{
    public class CategoryRequestDto
    {
        public required string Name { get; set; }
        public CategoryTypeEnum Type { get; set; } // income or expense
        public IFormFile? ImageURL { get; set; }
    }
}
