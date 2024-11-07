using Amazon.S3;
using Amazon.S3.Model;
using FileService.Application.Providers;
using FileService.Data.Models;
using FileService.Data.Shared;


namespace FileService.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private const int EXPIRATION_URL = 3;
    private readonly IAmazonS3 _client;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IAmazonS3 client, ILogger<MinioProvider> logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyList<string>>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IsBucketExist(filesList.Select(f => f.FileMetadata.BucketName), cancellationToken);

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
    

    public async Task<Result> RemoveFile(
        FileMetadata fileMetadata,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await IsBucketExist([fileMetadata.BucketName], cancellationToken);

            var key = fileMetadata.Name + "." + fileMetadata.Extension;

            var deleteRequest = new DeleteObjectRequest()
            {
                BucketName = fileMetadata.BucketName,
                Key = key
            };
            
            await _client.DeleteObjectAsync(deleteRequest, cancellationToken);
        }
        catch(Exception e)
        {
            _logger.LogError(e,"Fail to remove file in minio with path {path} in bucket {bucket}",
                fileMetadata.FullPath, fileMetadata.BucketName);
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }

        return Result.Success();
    }
    
    public async Task<Result<string>> GetFile(FileMetadata fileMetadata, CancellationToken cancellationToken)
    {
        try
        {
            var key = fileMetadata.Name + "." + fileMetadata.Extension;

            var request = new GetPreSignedUrlRequest
            {
                BucketName = fileMetadata.BucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddDays(EXPIRATION_URL)
            };

            var url = await _client.GetPreSignedURLAsync(request);
            
            if (url is null)
                return Error.NotFound("object.not.found", "File doesn`t exist in minio");
            
            return url;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Fail to get file in minio");
            return Error.Failure("file.get", "Fail to get file in minio");
        }
    }

    private async Task<Result<string>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var key = fileData.FileMetadata.Name + "." + fileData.FileMetadata.Extension;
        
        var putObjectRequest = new PutObjectRequest
        {
            BucketName = fileData.FileMetadata.BucketName,
            Key = key,
            InputStream = fileData.Stream
        };

        try
        {
            await _client.PutObjectAsync(putObjectRequest, cancellationToken);

            return key;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FileMetadata.FullPath,
                fileData.FileMetadata.BucketName);

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
        
        var response = await _client.ListBucketsAsync(cancellationToken);

        foreach (var bucketName in buckets)
        {
            var isExist = response.Buckets
                .Exists(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));

            if (!isExist)
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName
                };

                await _client.PutBucketAsync(request, cancellationToken);
            }
        }
    }
}