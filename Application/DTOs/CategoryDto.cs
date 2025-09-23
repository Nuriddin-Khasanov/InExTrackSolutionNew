using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class CategoryDto
    {
        public required string Name { get; set; }
        public CategoryTypeEnum Type { get; set; } // income or expense
        public IFormFile? ImageURL { get; set; }
    }
}
