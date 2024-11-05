namespace FileService.Options;

public class MongoDatabaseSettings
{
    public static readonly string Mongo = nameof(MongoDatabaseSettings);
    
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string FilesCollectionName { get; set; } = null!;
}