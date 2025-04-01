using System.Collections.Generic;
using FileService.Application.Repositories;
using FileService.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDatabaseSettings = FileService.Data.Options.MongoDatabaseSettings;

namespace FileService.Infrastructure.Repositories;

public class FilesDataRepository: IFilesDataRepository
{
    private readonly IMongoCollection<FileMetadata> _filesCollection;

    public FilesDataRepository(IMongoDatabase database, IOptions<MongoDatabaseSettings> settings)
    {
        _filesCollection = database.GetCollection<FileMetadata>(settings.Value.FilesCollectionName);
    }

    public async Task<List<FileMetadata>> Get(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default)
    {
        return await _filesCollection.Find(file => fileDataIds.Contains(file.Id)).ToListAsync(cancellationToken);
    }

    public async Task<FileMetadata?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _filesCollection.Find(file => file.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<FileMetadata> fileData, CancellationToken cancellationToken = default)
    {
        await _filesCollection.InsertManyAsync(fileData,cancellationToken: cancellationToken);
    }
    
    public async Task DeleteRangeAsync(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default)
    {
        await _filesCollection.DeleteManyAsync(file => fileDataIds.Contains(file.Id), cancellationToken);
    }
    
}