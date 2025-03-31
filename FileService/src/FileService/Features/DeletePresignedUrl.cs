using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Contract;
using FileService.Contract.Requests;
using FileService.Contract.Responses;
using FileService.Data.Models;


namespace FileService.Features;

public static class DeletePresignedUrl
{
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-for-deletion", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        DeletePresignedUrlRequest request,
        IFilesDataRepository filesDataRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        var fileMetadata = new FileMetadata
        {
            BucketName = request.BucketName,
            Key = $"{request.FileId}{request.Extension}",
        };
        
        var result = await provider.GetPresignedUrlForDelete(fileMetadata, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Errors);

        if (!Guid.TryParse(request.FileId, out var guid))
        {
            return Results.BadRequest();
        }
        
        await filesDataRepository.DeleteRangeAsync([guid], cancellationToken);
        
        var response = new GetDeletePresignedUrlResponse(result.Value);
        
        return Results.Ok(response);
    }
}