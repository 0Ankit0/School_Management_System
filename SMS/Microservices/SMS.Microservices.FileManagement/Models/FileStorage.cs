namespace SMS.Microservices.FileManagement.Models;

public class FileStorage
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public Guid UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public string ContentType { get; set; }
    public long Length { get; set; }
    public int Version { get; set; }
}
