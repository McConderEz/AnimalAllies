using AnimalAllies.Application.FileProvider;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Application.Providers;

public interface IFileProvider
{
    Task<Result<string>> UploadFile(FileData fileData, CancellationToken cancellationToken);
    Task<Result<string>> DeleteFile(FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task<Result<string>> GetFileByObjectName(FileMetadata fileMetadata, CancellationToken cancellationToken);
}