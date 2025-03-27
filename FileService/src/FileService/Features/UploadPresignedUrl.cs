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
        if (string.IsNullOrEmpty(request.ContentType))
            return Results.BadRequest("Content type is empty");

        var sanitizedFilename = Path.GetFileName(request.FileName);
        
        var key = Guid.NewGuid();

        var fileKey = $"{key}_{sanitizedFilename}";
        
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = sanitizedFilename,
            Prefix = request.Prefix,
            Key = fileKey
        };
        
        var result = await fileProvider.GetPresignedUrlForUpload(fileMetadata, cancellationToken);
        if (result.IsFailure)
            return Results.Problem(result.Errors.FirstOrDefault()!.ErrorMessage);

        var dbRecord = new FileMetadata
        {
            Id = Guid.NewGuid(),
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            DownloadUrl = result.Value,
            Extension = Path.GetExtension(request.FileName),
            Key = fileMetadata.Key,
            Name = sanitizedFilename
        };
        
        await repository.AddRangeAsync([dbRecord], cancellationToken);
        
        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(
            j => j.Execute(
                dbRecord.Id, dbRecord.BucketName, dbRecord.Key),
            TimeSpan.FromMinutes(5));
        
        return Results.Ok(new
        {
            FileId = dbRecord.Id,
            UploadUrl = result.Value,
        });
    }
}