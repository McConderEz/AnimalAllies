using System.Runtime.InteropServices.JavaScript;
using AnimalAllies.Application.FileProvider;
using AnimalAllies.Application.Providers;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;
using FileInfo = AnimalAllies.Application.FileProvider.FileInfo;
using IFileProvider = AnimalAllies.Application.Providers.IFileProvider;

namespace AnimalAllies.Infrastructure.Providers;

public class MinioProvider: IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyList<FilePath>>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IsBucketExist(filesList.Select(f => f.FileInfo.BucketName), cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Errors;

            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    
    public async Task<Result<string>> DeleteFile(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var objectExistArgs = new PresignedGetObjectArgs()
                .WithBucket(fileMetadata.BucketName)
                .WithObject(fileMetadata.ObjectName)
                .WithExpiry(60 * 60 * 24);

            var objectExist = await _minioClient.PresignedGetObjectAsync(objectExistArgs);
            
            if (string.IsNullOrWhiteSpace(objectExist))
            {
                return Error.NotFound("object.not.found", "File doesn`t exist in minio");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileMetadata.BucketName)
                .WithObject(fileMetadata.ObjectName);
            
            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            
            return fileMetadata.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Fail to delete file in minio");
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result> RemoveFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await IsBucketExist([fileInfo.BucketName], cancellationToken);

            var statArgs = new StatObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);


            var objectStat = await _minioClient.StatObjectAsync(statArgs, cancellationToken);
            
            if(objectStat is null)
                return Result.Success();
            
            var arg = new RemoveObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.Path);

            await _minioClient.RemoveObjectAsync(arg, cancellationToken);
        }
        catch(Exception e)
        {
            _logger.LogError(e,"Fail to remove file in minio with path {path} in bucket {bucket}",
                fileInfo.FilePath.Path, fileInfo.BucketName);
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }

        return Result.Success();
    }

    public Result<IReadOnlyCollection<string>> GetFiles()
    {
        var listObjectsArgs = new ListObjectsArgs()
            .WithBucket("photos")
            .WithRecursive(false);

        var objects = _minioClient.ListObjectsAsync(listObjectsArgs);

        List<string> paths = [];
        
        var subscription = objects.Subscribe(
            (item) => paths.Add(item.Key),
            ex => _logger.LogError(ex, "Failed to get files"),
            () => _logger.LogInformation("Successfully uploaded files"));

        return paths;
    }

    public async Task<Result<string>> GetFileByObjectName(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var objectExistArgs = new StatObjectArgs()
                .WithBucket(fileMetadata.BucketName)
                .WithObject(fileMetadata.ObjectName);

            var objectStat = await _minioClient.StatObjectAsync(objectExistArgs, cancellationToken);
            
            if (objectStat == null)
            {
                return Error.NotFound("object.not.found", "File doesn`t exist in minio");
            }
            
            return objectStat.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Fail to get file in minio");
            return Error.Failure("file.get", "Fail to get file in minio");
        }
    }

    private async Task<Result<FilePath>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FileInfo.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FileInfo.FilePath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FileInfo.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FileInfo.FilePath.Path,
                fileData.FileInfo.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task IsBucketExist(IEnumerable<string> bucketNames,CancellationToken cancellationToken)
    {
        HashSet<string> buckets = [..bucketNames];

        foreach (var bucketName in buckets)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }
}