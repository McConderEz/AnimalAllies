using Amazon.S3;
using Amazon.S3.Model;
using FileService.Api.Endpoints;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using FileService.Data.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Features;

public static class GetFilesByIds
{
    private record GetFilesByIdsRequest(
        IEnumerable<Guid> FileIds);
    
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
        
        files = files.Zip(urls.Value,(file,url) => 
        { 
            file.DownloadUrl = url;
            return file;
        }).ToList();
        
        return Results.Ok(files);
    }
}