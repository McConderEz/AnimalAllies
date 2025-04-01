using AnimalAllies.Core.DTOs.FileService;

namespace AnimalAllies.Accounts.Contracts.Requests;

public record AddAvatarRequest(Guid UserId, UploadFileDto UploadFileDto);