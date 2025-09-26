using Domain.Enums;

namespace Application.DTOs.Responses
{
    public class CategoryResponceDto
    {
        public required string Name { get; set; }
        public CategoryTypeEnum Type { get; set; } // income or expense
        public string? ImageURL { get; set; }
    }
}
