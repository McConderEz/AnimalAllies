using AnimalAllies.Application.Common;
using AnimalAllies.Application.Contracts.DTOs.Volunteer;
using AnimalAllies.Domain.Models;

namespace AnimalAllies.Application.Abstractions;

public interface IVolunteerService
{
    Task<Result<VolunteerId>> Create(CreateVolunteerRequest request);
    Task Delete(Guid id);
    Task Update(Volunteer entity);
    Task<Volunteer> GetById(Guid id);
    Task<List<Volunteer>> Get();
}