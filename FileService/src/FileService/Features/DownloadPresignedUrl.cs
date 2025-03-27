using Amazon.S3;
using Amazon.S3.Model;
using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using FileService.Data.Shared;

namespace FileService.Features;

public static class DownloadPresignedUrl
{
    private record DownloadPresignedUrlRequest(
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
            app.MapPost("files/{key:guid}/presigned-for-downloading", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DownloadPresignedUrlRequest request,
        Guid key,
        IFilesDataRepository filesDataRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            ContentType = request.ContentType,
            Name = request.FileName,
            Prefix = request.Prefix,
            Key = $"{key}.{request.Extension}",
            Extension = request.Extension
        };
        
        var result = await provider.GetPresignedUrlForDownload(fileMetadata, cancellationToken); 
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}