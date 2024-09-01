using System.Runtime.InteropServices.JavaScript;
using AnimalAllies.Application.FileProvider;
using AnimalAllies.Application.Providers;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;
using IFileProvider = AnimalAllies.Application.Providers.IFileProvider;

namespace AnimalAllies.Infrastructure.Providers;

public class MinioProvider: IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }
    
    public async Task<Result<string>> UploadFile(FileData fileData,CancellationToken cancellationToken = default)
    {
        try
        {
            var isBucketExist = await IsBucketExist(fileData.BucketName,cancellationToken);

            if (isBucketExist.IsFailure)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(fileData.BucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }

            var path = Guid.NewGuid();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(path.ToString());

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return result.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
    }

    public async Task<Result<string>> DeleteFile(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var isBucketExist = await IsBucketExist(fileMetadata.BucketName,cancellationToken);

            if (isBucketExist.IsFailure)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(fileMetadata.BucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
            
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

    public async Task<Result<string>> GetFileByObjectName(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var isBucketExist = await IsBucketExist(fileMetadata.BucketName,cancellationToken);

            if (isBucketExist.IsFailure)
                return Error.NotFound("bucket.not.found", "bucket doesn`t exist");

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

    private async Task<Result> IsBucketExist(string bucketName,CancellationToken cancellationToken)
    {
        var bucketExistArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

        if (bucketExist == false)
        {
            return Error.NotFound("bucket.not.found", "Bucket doesn`t exist");
        }

        return Result.Success();
    }
}