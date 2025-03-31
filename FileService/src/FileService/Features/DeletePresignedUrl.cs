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
        DeletePresignedUrlsRequest request,
        IFilesDataRepository filesDataRepository,
        IFileProvider provider,
        CancellationToken cancellationToken = default)
    {
        List<FileMetadata> fileMetadatas = [];
        fileMetadatas.AddRange(request.Requests
            .Select(file => 
                new FileMetadata { BucketName = file.BucketName, Key = $"{file.FileId}{file.Extension}", }));

        var result = await provider.GetPresignedUrlsForDeleteParallel(fileMetadatas, cancellationToken);
        if (result.IsFailure)
            return Results.BadRequest(result.Errors);

        List<Guid> guids = [];
        foreach (var file in request.Requests)
        {
            if (!Guid.TryParse(file.FileId, out var guid))
            {
                return Results.BadRequest();
            }
            
            guids.Add(guid);
        }
        
        await filesDataRepository.DeleteRangeAsync(guids, cancellationToken);
        
        var response = new GetDeletePresignedUrlsResponse(result.Value);
        
        return Results.Ok(response);
    }
}