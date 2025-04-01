using Amazon.S3;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Models;
using Hangfire;

namespace FileService.Jobs;

public class ConfirmConsistencyJob(
    IFilesDataRepository filesRepository,
    IFileProvider fileProvider,
    ILogger<ConfirmConsistencyJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(IEnumerable<FileMetadata> records, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Start ConfirmConsistencyJob for {count} files", records.Count());

            var inconsistencies = new List<InconsistencyReport>();
            
            await Parallel.ForEachAsync(records, cancellationToken, async (record, ct) =>
            {
                try
                {
                    var cloudMetadata = await fileProvider.GetObjectMetadata(record.BucketName, record.Key, ct);
                    var dbRecord = await filesRepository.GetById(record.Id, ct);

                    if (dbRecord is null)
                    {
                        inconsistencies.Add(new InconsistencyReport(
                            record.Id,
                            record.Extension,
                            record.BucketName,
                            "MongoDB record not found",
                            InconsistencyType.MissingInDatabase));
                        return;
                    }

                    if (cloudMetadata is null)
                    {
                        inconsistencies.Add(new InconsistencyReport(
                            record.Id,
                            record.Extension,
                            record.BucketName,
                            "File not found in cloud storage",
                            InconsistencyType.MissingInCloud));
                        return;
                    }

                    if (cloudMetadata.Key != dbRecord.Key ||
                        cloudMetadata.BucketName != dbRecord.BucketName)
                    {
                        inconsistencies.Add(new InconsistencyReport(
                            record.Id,
                            record.Extension,
                            record.BucketName,
                            "Metadata mismatch between cloud and database",
                            InconsistencyType.MetadataMismatch));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error checking consistency for file {fileId}", record.Id);
                    inconsistencies.Add(new InconsistencyReport(
                        record.Id,
                        record.Extension,
                        record.BucketName,
                        $"Check failed: {ex.Message}",
                        InconsistencyType.CheckError));
                }
            });
            
            await ProcessInconsistencies(inconsistencies);

            logger.LogInformation("Completed ConfirmConsistencyJob. Found {count} inconsistencies",
                inconsistencies.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error in ConfirmConsistencyJob");
            throw;
        }
    }

    private async Task ProcessInconsistencies(IEnumerable<InconsistencyReport> reports)
    {
        var missingInDb = reports
            .Where(r => r.Type == InconsistencyType.MissingInDatabase);
        var missingInCloud = reports
            .Where(r => r.Type == InconsistencyType.MissingInCloud);
        var metadataMismatches = reports
            .Where(r => r.Type == InconsistencyType.MetadataMismatch);
        
        foreach (var report in missingInDb)
        {
            try
            {
                var metadata = new FileMetadata
                {
                    BucketName = report.BucketName,
                    Key = $"{report.FileId}{report.Extension}"
                };

                await fileProvider.DeleteFile(metadata);
                logger.LogWarning("Deleted orphaned cloud file {fileId}", report.FileId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete orphaned file {fileId}", report.FileId);
            }
        }
        
        if (missingInCloud.Any())
        {
            await filesRepository.DeleteRangeAsync(missingInCloud.Select(r => r.FileId));
            logger.LogWarning("Deleted {count} database records without cloud files",
                missingInCloud.Count());
        }
        
        foreach (var report in metadataMismatches)
        {
            logger.LogWarning("Metadata mismatch for file {fileId}: {message}",
                report.FileId, report.Message);
        }
    }

    private record InconsistencyReport(
        Guid FileId,
        string Extension,
        string BucketName,
        string Message,
        InconsistencyType Type);

    private enum InconsistencyType
    {
        MissingInDatabase,
        MissingInCloud,
        MetadataMismatch,
        CheckError
    }
}