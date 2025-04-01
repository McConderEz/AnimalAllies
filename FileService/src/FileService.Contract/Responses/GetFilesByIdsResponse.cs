namespace FileService.Contract.Responses;

public record ResponseData(string UploadUrl, Guid FileId, string Extension);

public record GetFilesByIdsResponse(IEnumerable<ResponseData> UploadUrls);
