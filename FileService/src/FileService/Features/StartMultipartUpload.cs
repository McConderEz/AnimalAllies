using Amazon.S3;
using Amazon.S3.Model;

namespace FileService.Features;

public static class StartMultipartUpload
{
    private record StartMultipartUploadRequest(
        string FileName,
        string ContentType,
        long size);
    
    public sealed class Endpoint
    {
                
    }

    private static async Task<IResult> Handler(
        StartMultipartUploadRequest request,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();

        return null;
    }
}