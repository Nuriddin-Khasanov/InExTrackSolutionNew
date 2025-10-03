namespace Application.DTOs.Responses
{
    public class TransactionResponseDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}
