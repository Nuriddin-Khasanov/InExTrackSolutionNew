using Domain.Commons;

namespace Domain.Models;

public class CategoryFile : DataFile
{
    public Guid CategoryId { get; set; }
    public Category? User { get; set; }
}
