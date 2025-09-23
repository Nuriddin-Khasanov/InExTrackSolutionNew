namespace Application.DTOs
{
    public class TransactionDto
    {
        public Guid UserCategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }
    }
}
