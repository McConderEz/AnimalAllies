using AnimalAllies.Application.Abstractions.Volunteer;
using AnimalAllies.Application.Common;
using AnimalAllies.Domain.Models;

namespace AnimalAllies.Application.Services;

public class VolunteerService: IVolunteerService
{
    private readonly IVolunteerRepository _repository;

    public VolunteerService(IVolunteerRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task Create(Volunteer entity)
    {
        await _repository.Create(entity);
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Volunteer entity)
    {
        throw new NotImplementedException();
    }

    public Task<Volunteer> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Volunteer>> Get()
    {
        throw new NotImplementedException();
    }
}