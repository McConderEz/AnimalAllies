using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Data.Models;

namespace FileService.Features;

public static class StartMultipartUpload
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multi-part-uploading", Handler);
        }
    }

    private static async Task<IResult> Handler(
        StartMultipartUploadRequest request,
        IFileProvider fileProvider,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            FileName = request.FileName,
            Prefix = request.Prefix,
            Key = $"{request.Prefix}/{key}"
        };

        var response = await fileProvider.InitialMultipartUpload(fileMetadata, cancellationToken);
        
        return Results.Ok(new
        {
            key,
            uploadId = response.UploadId
        });
    }
}