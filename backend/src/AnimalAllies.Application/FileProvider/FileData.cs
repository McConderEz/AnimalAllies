using System.IO;
using AnimalAllies.Domain.Models.Volunteer.Pet;

namespace AnimalAllies.Application.FileProvider;

public record FileData(Stream Stream, FileInfo FileInfo);

public record FileInfo(FilePath FilePath, string BucketName);