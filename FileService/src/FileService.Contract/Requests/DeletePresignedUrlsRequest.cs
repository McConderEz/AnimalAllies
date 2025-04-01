namespace FileService.Contract.Requests;

public record DeletePresignedUrlsRequest(IEnumerable<DeletePresignedUrlRequest> Requests);
