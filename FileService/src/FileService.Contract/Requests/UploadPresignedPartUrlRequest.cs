namespace FileService.Contract.Requests;

public record UploadPresignedPartUrlRequest(
    string UploadId, 
    int PartNumber, 
    string BucketName,
    string ContentType, 
    string Prefix,
    string FileName);