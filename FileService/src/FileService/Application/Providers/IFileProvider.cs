using FileService.Data.Models;
using FileService.Data.Shared;

namespace FileService.Application.Providers;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<string>>> UploadFiles(IEnumerable<FileData> filesData, CancellationToken cancellationToken);
    Task<Result<string>> GetFile(FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task<Result> RemoveFile(FileMetadata fileMetadata, CancellationToken cancellationToken);
}