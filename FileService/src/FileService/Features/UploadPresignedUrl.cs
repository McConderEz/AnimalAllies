using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Contract.Responses;
using FileService.Data.Models;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class UploadPresignedUrl
{
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
        
        var id = Guid.NewGuid();
        
        var extension = Path.GetExtension(request.FileName);

        var fileKey = $"{id}{extension}";
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            FileName = request.FileName,
            Key = fileKey
        };
        
        var result = await fileProvider.GetPresignedUrlForUpload(fileMetadata, cancellationToken);
        if (result.IsFailure)
            return Results.Problem(result.Errors.FirstOrDefault()!.ErrorMessage);

        var dbRecord = new FileMetadata
        {
            Id = id,
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Extension = Path.GetExtension(request.FileName),
            Key = fileMetadata.Key,
            FileName = request.FileName,
        };
        
        await repository.AddRangeAsync([dbRecord], cancellationToken);
        
        BackgroundJob.Schedule<ConfirmConsistencyJob>(
            j => j.Execute(new[] {dbRecord}, cancellationToken),
            TimeSpan.FromMinutes(3));
        
        var response = new GetUploadPresignedUrlResponse(dbRecord.Id, dbRecord.Extension, result.Value);
        
        return Results.Ok(response);
    }
}