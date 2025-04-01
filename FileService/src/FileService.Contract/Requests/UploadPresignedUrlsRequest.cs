namespace FileService.Contract.Requests;

public record UploadPresignedUrlsRequest(IEnumerable<UploadPresignedUrlRequest> Files);
