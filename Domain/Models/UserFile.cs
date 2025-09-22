using InExTrack.Domain.Commons;

namespace InExTrack.Domain.Models
{
    public class UserFile: DataFile
    {
        public required Guid UserId { get; set; }
    }
}
