using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Application.Repositories;

public interface IVolunteerRepository
{
    Task<Result<VolunteerId>> Create(Volunteer entity, CancellationToken cancellationToken = default);
    Task<Result<VolunteerId>> Delete(Volunteer entity, CancellationToken cancellationToken = default);
    Task<Result<VolunteerId>> Save(Volunteer entity, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetById(VolunteerId id, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber phone, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetByEmail(Email email, CancellationToken cancellationToken = default);
    Task<Result<List<Volunteer>>> Get(CancellationToken cancellationToken = default);
}