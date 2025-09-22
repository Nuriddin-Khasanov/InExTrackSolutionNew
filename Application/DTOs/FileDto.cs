namespace InExTrack.Application.DTOs
{
    public class FileDto
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string Extension { get; set; }
        public long Size { get; set; }
    }
}
