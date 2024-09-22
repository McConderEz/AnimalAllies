using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;
