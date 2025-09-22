namespace InExTrack.Domain.Commons
{
    public abstract class DataFile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required long Size { get; set; }
        public string? Extension { get; set; }
    }
}
