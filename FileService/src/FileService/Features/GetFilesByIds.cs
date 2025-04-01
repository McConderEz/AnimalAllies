using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Contract.Requests;
using FileService.Contract.Responses;

namespace FileService.Features;

public static class GetFilesByIds
{
    public sealed class Endpoint: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/files-by-ids", Handler);
        }
    }

    private static async Task<IResult> Handler( 
        GetFilesByIdsRequest request,
        IFileProvider fileProvider,
        IFilesDataRepository filesDataRepository,
        CancellationToken cancellationToken = default)
    {
        var files = await filesDataRepository.Get(request.FileIds, cancellationToken);

        var urls = await fileProvider.DownloadFiles(files, cancellationToken);
        if (urls.IsFailure)
            return Results.Conflict(error: urls.Errors);
        
        var responseData = files.Zip(urls.Value,(file,url) => 
        { 
            var response = new ResponseData(url, file.Id, file.Extension);
            return response;
        }).ToList();
        
        return Results.Ok(responseData);
    }
}