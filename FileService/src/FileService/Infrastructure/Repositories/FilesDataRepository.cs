﻿using FileService.Models;
using System.Collections.Generic;
using MongoDB.Driver;

namespace FileService.Infrastructure.Repositories;

public class FilesDataRepository
{
    private readonly ApplicationDbContext _context;

    public FilesDataRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FileMetadata>> Get(CancellationToken cancellationToken = default)
    {
        return await _context.FileDataCollection.Find(file => true).ToListAsync(cancellationToken);
    }

    public async Task<FileMetadata> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FileDataCollection.Find(file => file.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<FileMetadata> fileData, CancellationToken cancellationToken = default)
    {
        await _context.FileDataCollection.InsertManyAsync(fileData,null, cancellationToken);
    }
    
    public async Task DeleteRangeAsync(IEnumerable<Guid> fileDataIds, CancellationToken cancellationToken = default)
    {
        await _context.FileDataCollection.DeleteManyAsync(file => fileDataIds.Contains(file.Id), cancellationToken);
    }
    
}