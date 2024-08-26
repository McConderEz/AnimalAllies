using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Application.Repositories;

public interface IVolunteerRepository
{
    Task<Result<VolunteerId>> Create(Volunteer entity, CancellationToken cancellationToken = default);
    Task Delete(Volunteer entity, CancellationToken cancellationToken = default);
    Task<Result<VolunteerId>> Update(Volunteer entity, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetById(VolunteerId id, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber phone, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetByEmail(Email email, CancellationToken cancellationToken = default);
    Task<Result<List<Volunteer>>> Get(CancellationToken cancellationToken = default);
}