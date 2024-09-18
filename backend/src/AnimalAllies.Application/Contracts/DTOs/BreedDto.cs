namespace AnimalAllies.Application.Contracts.DTOs;

public class BreedDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid SpeciesId { get; set; }
}