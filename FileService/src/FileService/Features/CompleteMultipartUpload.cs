using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
    private record PartETagInfo(int PartNumber, string ETag);
    
    private record CompleteMultipartUploadRequest(string UploadId,string BucketName, string Key, List<PartETagInfo> Parts);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/complete-multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(
        CompleteMultipartUploadRequest request,
        IFileProvider fileProvider,
        IFilesDataRepository repository,
        CancellationToken cancellationToken = default)
    {
        var fileId = Guid.NewGuid();
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Key = request.Key,
            UploadId = request.UploadId,
            ETags =  request.Parts.Select(e => new ETagInfo{PartNumber = e.PartNumber,ETag = e.ETag})
        };
        
        var response = await fileProvider
            .CompleteMultipartUpload(fileMetadata, cancellationToken);

        var metaDataResponse =
            await fileProvider.GetObjectMetadata(fileMetadata.BucketName, fileMetadata.Key, cancellationToken);

        metaDataResponse.Id = fileId;
        
        await repository.AddRangeAsync([metaDataResponse], cancellationToken);

        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(
            j => j.Execute(new []{metaDataResponse}, cancellationToken),
            TimeSpan.FromMinutes(3));

        BackgroundJob.Delete(jobId);
        
        return Results.Ok(new
        { 
            response
        });
    }
}