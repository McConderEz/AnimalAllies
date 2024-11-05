using FileService.Infrastructure;

namespace FileService.Services;

public class FilesDataService
{
    private readonly FilesDataService _filesDataService;
    private readonly MinioProvider _minioProvider;

    public FilesDataService(FilesDataService filesDataService, MinioProvider minioProvider)
    {
        _filesDataService = filesDataService;
        _minioProvider = minioProvider;
    }
}