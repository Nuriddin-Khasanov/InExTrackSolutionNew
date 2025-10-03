using Domain.Enums;

namespace Application.DTOs.Responses
{
    public class CategoryResponceDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public CategoryTypeEnum Type { get; set; } // income or expense
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
    }
}
