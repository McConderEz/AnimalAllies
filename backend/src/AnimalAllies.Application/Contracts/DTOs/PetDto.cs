using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs;

public class PetDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string HealthInformation { get; init; } = string.Empty;
    public double Weight { get; init; } 
    public double Height { get; init; }
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public string Description { get; init; } = string.Empty; 
    public DateOnly BirthDate { get; init; } 
    public DateTime CreationTime { get; init; }
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string HelpStatus { get; init; } = string.Empty;
    public Guid VolunteerId { get; init; }
    public Guid SpeciesId { get; init; }
    public Guid BreedId { get; init; }
    //public RequisiteDto[] Requisites { get; set; } = [];
    //public PetPhotoDto[] Photos { get; set; } = [];
}