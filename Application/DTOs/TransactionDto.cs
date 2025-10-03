namespace Application.DTOs
{
    public class TransactionDto
    {
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}
