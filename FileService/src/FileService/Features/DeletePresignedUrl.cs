using Amazon.S3;
using Amazon.S3.Model;
using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using FileService.Data.Shared;

namespace FileService.Features;

public static class DeletePresignedUrl
{
    private record DeletePresignedUrlRequest(
        string BucketName,
        string FileName, 
        string Extension,
        string ContentType,
        long Size);
    
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/presigned-for-deletion", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DeletePresignedUrlRequest request,
        Guid key,
        IFilesDataRepository filesDataRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Name = request.FileName,
            Key = $"{key}.{request.Extension}",
            Extension = request.Extension,
            ContentType = request.ContentType
        };
        
        var result = await provider.GetPresignedUrlForDelete(fileMetadata, cancellationToken); 
        
        return Results.Ok(new
        {
            key,
            url = result.Value
        });
    }
}