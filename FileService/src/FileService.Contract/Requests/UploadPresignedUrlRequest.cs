namespace FileService.Contract.Requests;

public record UploadPresignedUrlRequest(
    string BucketName,
    string FileName, 
    string ContentType);