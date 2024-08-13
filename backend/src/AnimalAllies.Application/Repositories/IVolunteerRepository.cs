using AnimalAllies.Domain.Models;

namespace AnimalAllies.Application.Common;

public interface IVolunteerRepository
{
    Task Create(Volunteer entity);
    Task Delete(Guid id);
    Task Update(Volunteer entity);
    Task<Volunteer> GetById(Guid id);
    Task<List<Volunteer>> Get();
}