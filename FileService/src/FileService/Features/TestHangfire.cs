using FileService.Api.Endpoints;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class TestHangfire
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("test", Handler);
        }
    }

    private static IResult Handler(
        CancellationToken cancellationToken)
    {
        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(j => j.Execute(Guid.NewGuid(), "string","key"), TimeSpan.FromSeconds(5));

        return Results.Ok(jobId);
    }
}