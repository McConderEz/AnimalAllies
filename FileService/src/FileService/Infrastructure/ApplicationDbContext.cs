using FileService.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDatabaseSettings = FileService.Data.Options.MongoDatabaseSettings;

namespace FileService.Infrastructure;

public class ApplicationDbContext
{
    private readonly MongoDatabaseSettings _options;
    private readonly IMongoDatabase _database;

    public ApplicationDbContext(IOptions<MongoDatabaseSettings> options)
    {
        _options = options.Value;
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<FileMetadata> FileDataCollection =>
        _database.GetCollection<FileMetadata>(_options.FilesCollectionName);
}