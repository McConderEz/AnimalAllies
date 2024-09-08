using AnimalAllies.Domain.Models.Volunteer.Pet;

namespace AnimalAllies.Application.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);