using AnimalAllies.Application.Common;
using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AnimalAllies.Infrastructure.Repositories;

public class VolunteerRepository: IVolunteerRepository
{
    private protected readonly AnimalAlliesDbContext _context;
    
    public VolunteerRepository(AnimalAlliesDbContext context)
    {
        _context = context;
    }
    
    public async Task<VolunteerId> Create(Volunteer entity)
    {
        
        if (entity == null)
            throw new ArgumentNullException($"{nameof(entity)} cannot be null!");
        
        await _context.Volunteers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
        
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