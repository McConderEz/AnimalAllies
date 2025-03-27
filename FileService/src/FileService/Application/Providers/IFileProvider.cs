using Amazon.S3.Model;
using FileService.Data.Models;
using FileService.Data.Shared;

namespace FileService.Application.Providers;

public interface IFileProvider
{
    Task<InitiateMultipartUploadResponse> InitialMultipartUpload(FileMetadata fileMetadata,
        CancellationToken cancellationToken = default);
    Task<Result<string>> GetPresignedUrlPartForUpload(
        FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task<CompleteMultipartUploadResponse> CompleteMultipartUpload(
        FileMetadata fileMetadata, CancellationToken cancellationToken = default);
    Task<Result<string>> GetPresignedUrlForUpload(FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task<Result<string>> GetPresignedUrlForDownload(
        FileMetadata fileMetadata,SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken);
    Task<Result<string>> GetPresignedUrlForDownload(
        FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task<FileMetadata> GetObjectMetadata(
        string bucketName, string key, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<string>>> DownloadFiles(
        IEnumerable<FileMetadata> filesMetadata, CancellationToken cancellationToken = default);
    Task<Result<string>> GetPresignedUrlForDelete(FileMetadata fileMetadata, CancellationToken cancellationToken);
    Task DeleteFile(FileMetadata fileMetadata, CancellationToken cancellationToken = default);
}