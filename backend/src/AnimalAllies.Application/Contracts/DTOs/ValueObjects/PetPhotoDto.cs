namespace AnimalAllies.Application.Contracts.DTOs.ValueObjects;

public class PetPhotoDto
{
    public string Path { get; init; } = string.Empty;
    public bool IsMain { get; set; } 
}