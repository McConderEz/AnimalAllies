using AnimalAllies.Application.FileProvider;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken);
    Task<Result<string>> DeleteFile(FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task<Result<string>> GetFileByObjectName(FileMetadata fileMetadata, CancellationToken cancellationToken);
}