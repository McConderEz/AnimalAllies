using Amazon.S3;
using Amazon.S3.Model;
using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using FileService.Data.Shared;

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
        IFilesDataRepository filesDataRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {

        var key = Guid.NewGuid();
        
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{key}.{request.Extension}"
        };
        
        var result = await provider.GetPresignedUrlForUpload(fileMetadata, cancellationToken); 
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}