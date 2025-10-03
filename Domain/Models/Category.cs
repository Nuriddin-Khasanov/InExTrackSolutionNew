using Domain.Commons;
using Domain.Enums;

namespace Domain.Models;

public class Category : Entity
{
    public Guid UserId { get; set; }

    public required string Name { get; set; }

    public CategoryTypeEnum Type { get; set; } // income or expense

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public CategoryFile? Image { get; set; }

    public User User { get; set; } = null!;

    public List<Transaction_> Transactions { get; set; } = [];

}

