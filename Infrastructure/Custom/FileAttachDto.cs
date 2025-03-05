namespace Infrastructure.Custom
{
    public class FileAttachDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public long FileSize { get; set; }
    }
}
