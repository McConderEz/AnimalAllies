using FileService.Infrastructure;
using FileService.Infrastructure.Repositories;

namespace FileService.Services;

public class FilesDataService
{
    private readonly FilesDataRepository _filesDataRepository;
    private readonly MinioProvider _minioProvider;

    public FilesDataService(FilesDataRepository filesDataRepository, MinioProvider minioProvider)
    {
        _filesDataRepository = filesDataRepository;
        _minioProvider = minioProvider;
    }
    
    //TODO: Реализовать 
}