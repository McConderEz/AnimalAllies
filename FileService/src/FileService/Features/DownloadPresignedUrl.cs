using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Contract.Responses;
using FileService.Data.Models;

namespace FileService.Features;

public static class DownloadPresignedUrl
{
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-for-downloading", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DownloadPresignedUrlRequest request,
        IFilesDataRepository filesDataRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Key = $"{request.FileId}.{request.Extension}",
        };
        
        var result = await provider.GetPresignedUrlForDownload(fileMetadata, cancellationToken); 
        if(result.IsFailure)
            return Results.BadRequest(result.Errors);

        var response = new GetDownloadPresignedUrlResponse(result.Value);
        
        return Results.Ok(response);
    }
}