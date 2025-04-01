
namespace FileService.Contract.Responses;

public record GetUploadPresignedUrlResponse(Guid FileId, string Extension,string UploadUrl);