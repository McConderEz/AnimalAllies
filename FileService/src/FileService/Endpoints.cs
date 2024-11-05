namespace FileService;

public static class Endpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/test", () => "Test");
    }
}