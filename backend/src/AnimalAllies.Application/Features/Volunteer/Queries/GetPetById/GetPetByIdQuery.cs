using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;
