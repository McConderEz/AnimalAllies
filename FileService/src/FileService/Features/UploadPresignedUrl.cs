using Amazon.S3;
using Amazon.S3.Model;
using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using FileService.Data.Shared;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(
        string BucketName,
        string FileName, 
        string ContentType,
        string Prefix,
        string Extension,
        long Size);
    
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-for-uploading", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        UploadPresignedUrlRequest request,
        IFilesDataRepository repository,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();

        var fileId = Guid.NewGuid();
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{key}.{request.Extension}"
        };
        
        var result = await fileProvider.GetPresignedUrlForUpload(fileMetadata, cancellationToken); 
        
        var metaDataResponse =
            await fileProvider.GetObjectMetadata(fileMetadata.BucketName, fileMetadata.Key, cancellationToken);

        metaDataResponse.Id = fileId;
        
        await repository.AddRangeAsync([metaDataResponse], cancellationToken);
        
        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(
            j => j.Execute(
                metaDataResponse.Id,metaDataResponse.BucketName, metaDataResponse.Key),
            TimeSpan.FromMinutes(1));

        BackgroundJob.Delete(jobId);
        
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}