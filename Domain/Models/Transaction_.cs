using Domain.Commons;

namespace Domain.Models;

public class Transaction_ : Entity
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public required User User { get; set; }
    public required Category Category { get; set; }
}

