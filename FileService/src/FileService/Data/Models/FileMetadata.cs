using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Data.Models;

public class FileMetadata
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    [BsonElement("name")]
    public string FileName { get; set; } = string.Empty;
    [BsonElement("extension")]
    public string Extension { get; set; } = string.Empty;
    [BsonElement("content_type")]
    public string ContentType { get; set; } = string.Empty;
    [BsonIgnore]
    public string FullPath { get; set; } = string.Empty;
    [BsonElement("size")]
    public long Size { get; set; }
    [BsonElement("bucket_name")]
    public string BucketName { get; set; } = string.Empty;
    [BsonElement("prefix")]
    public string Prefix { get; set; } = string.Empty;
    [BsonElement("key")]
    public string Key { get; set; } = string.Empty;
    [BsonIgnore]
    public string UploadId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int PartNumber { get; set; }
    public IEnumerable<ETagInfo>? ETags;
}