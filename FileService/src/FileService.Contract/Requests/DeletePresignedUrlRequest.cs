namespace FileService.Contract.Requests;

public record DeletePresignedUrlRequest(
    string BucketName,
    string FileId, 
    string Extension);