using Domain.Enums;

namespace Application.DTOs
{
    public class TransactionFilterDto
    {
        public Guid? CategoryId { get; set; }
        public CategoryTypeEnum? CategoryType { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Search { get; set; }
    }
}
