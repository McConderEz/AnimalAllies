using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Data.Models;

namespace FileService.Features;

public static class UploadPresignedPartUrl
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-part", Handler);
        }
    }

    private static async Task<IResult> Handler(
        UploadPresignedPartUrlRequest request,
        Guid key,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            FileName = request.FileName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}",
            UploadId = request.UploadId,
            PartNumber = request.PartNumber
        };
        
        
        var response = await fileProvider
            .GetPresignedUrlPartForUpload(fileMetadata, cancellationToken);
        
        return Results.Ok(new
        { 
            response
        });
    }
}