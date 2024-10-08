using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetPetsBySpeciesId;

//TODO: Пока без сортировки и фильтрации
public record GetPetsBySpeciesIdQuery(
    Guid SpeciesId) : IQuery;
