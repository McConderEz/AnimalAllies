namespace FileService.Models;

public class FileMetadata
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public string StorageInfo { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
}