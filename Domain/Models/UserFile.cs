using Domain.Commons;

namespace Domain.Models;

public class UserFile: DataFile
{
    public required Guid UserId { get; set; }
    public User? User { get; set; }
}
