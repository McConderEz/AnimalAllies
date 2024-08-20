using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Application.Repositories;

public interface IVolunteerRepository
{
    Task<Result<VolunteerId>> Create(Volunteer entity, CancellationToken cancellationToken = default);
    Task Delete(Guid id);
    Task Update(Volunteer entity);
    Task<Result<Volunteer>> GetById(VolunteerId id);
    Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber phone);
    Task<Result<Volunteer>> GetByEmail(Email email);
    Task<Result<List<Volunteer>>> Get();
}