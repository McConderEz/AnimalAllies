using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs;

public class PetDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string HealthInformation { get; init; } = string.Empty;
    public double Weight { get; set; } 
    public double Height { get; set; }
    public bool IsCastrated { get; set; }
    public bool IsVaccinated { get; set; }
    public string Description { get; init; } = string.Empty; 
    public DateOnly BirthDate { get; init; } 
    public DateTime CreationTime { get; init; }
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string HelpStatus { get; init; } = string.Empty;
    public Guid VolunteerId { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid BreedId { get; set; }
    //public RequisiteDto[] Requisites { get; set; } = [];
    //public PetPhotoDto[] Photos { get; set; } = [];
}