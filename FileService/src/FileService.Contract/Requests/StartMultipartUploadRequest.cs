namespace FileService.Contract.Requests;

public record StartMultipartUploadRequest(
    string BucketName,
    string FileName, 
    string ContentType,
    string Prefix,
    long Size);