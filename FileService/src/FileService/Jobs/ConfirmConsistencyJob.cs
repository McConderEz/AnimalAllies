using Amazon.S3;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using Hangfire;

namespace FileService.Jobs;

public class ConfirmConsistencyJob(
    IFilesDataRepository filesRepository,
    IFileProvider fileProvider,
    ILogger<ConfirmConsistencyJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(Guid fileId,string bucketName, string key)
    {
        try
        {
            logger.LogInformation("Start ConfirmConsistencyJob with {fileId} and {key}", fileId, key);

            var metadata = await fileProvider.GetObjectMetadata(bucketName, key);

            var mongoData = await filesRepository.GetById(fileId);

            if (mongoData is null)
            {
                logger.LogWarning("MongoDB record not found for fileId: {fileId}." +
                                  " Deleting file from cloud storage.", fileId);
                await fileProvider.DeleteFile(metadata);
                return;
            }

            if (metadata.Key != mongoData.Key)
            {
                logger.LogWarning("Metadata key does not match MongoDB data." +
                                  " Deleting file from cloud storage and MongoDB record.");

                await fileProvider.DeleteFile(metadata);
                await filesRepository.DeleteRangeAsync(new[] { fileId });
            }

            logger.LogInformation("End ConfirmConsistencyJob");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot check consistency, because " + ex.Message);
        }
    }
}