namespace Domain.Commons;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
