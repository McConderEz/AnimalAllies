using Amazon.S3;
using Amazon.S3.Model;

namespace FileService.Api.Extensions;

/// <summary>
/// Сидирование бакета в S3, если он не существует
/// </summary>
/// <param name="fileProvider"></param>
/// <param name="logger"></param>
public class Seeding(IAmazonS3 fileProvider, ILogger<Seeding> logger)
{
    private const string BUCKET_NAME = "photos";

    public async Task SeedBucket()
    {
        logger.LogInformation("Start seeding bucket to S3");
        var buckets = await fileProvider.ListBucketsAsync();

        if (!buckets.Buckets.Exists(b => b.BucketName
                .Equals(BUCKET_NAME, StringComparison.OrdinalIgnoreCase)))
        {
            var bucketRequest = new PutBucketRequest
            {
                BucketName = BUCKET_NAME
            };

            await fileProvider.PutBucketAsync(bucketRequest);
            
            logger.LogInformation("Added bucket {bucketName} to S3", bucketRequest.BucketName);
        }
        
        logger.LogInformation("End seeding bucket to S3");
    }
    
}