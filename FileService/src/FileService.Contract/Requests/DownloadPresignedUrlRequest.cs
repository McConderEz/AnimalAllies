namespace FileService.Contract.Requests;

public record DownloadPresignedUrlRequest(
    string BucketName,
    string FileId,
    string Extension);