using InExTrack.Domain.Commons;

namespace InExTrack.Domain.Models
{
    public class UserCategory: Entity
    {
        public required Guid UserId { get; set; }
        public required Guid CategoryId { get; set; }
        public bool IsActive { get; set; } = true;

        public User? User { get; set; }
        public Category? Category { get; set; }
        public Transaction_? Transaction_ { get; set; }
    }
}
