using Amazon.S3;
using Amazon.S3.Model;
using Minio;
using Minio.DataModel.Args;

namespace FileService.Api.Extensions;

/// <summary>
/// Сидирование бакета в S3, если он не существует
/// </summary>
/// <param name="fileProvider"></param>
/// <param name="logger"></param>
public class Seeding(IMinioClient fileProvider, ILogger<Seeding> logger)
{
    private const string BUCKET_NAME = "photos";

    public async Task SeedBucket()
    {
        logger.LogInformation("Start seeding bucket to S3");
        var buckets = await fileProvider.ListBucketsAsync();
        
        if (!buckets.Buckets.Any(b => b.Name.Equals(BUCKET_NAME, StringComparison.OrdinalIgnoreCase)))
        {
            var makeBucket = new MakeBucketArgs()
                .WithBucket(BUCKET_NAME);

            await fileProvider.MakeBucketAsync(makeBucket);
            
            logger.LogInformation("Added bucket {bucketName} to S3", BUCKET_NAME);
        }
        
        logger.LogInformation("End seeding bucket to S3");
    }
    
}