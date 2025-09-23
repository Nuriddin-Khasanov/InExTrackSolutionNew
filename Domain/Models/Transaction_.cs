using Domain.Commons;

namespace Domain.Models;

public class Transaction_ : Entity
{
    public Guid UserCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }

    public required List<UserCategory> UserCategories { get; set; }
}
