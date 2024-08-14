using AnimalAllies.Application.Common;
using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AnimalAllies.Infrastructure.Repositories;

public class VolunteerRepository: IVolunteerRepository
{
    private readonly AnimalAlliesDbContext _context;
    
    public VolunteerRepository(AnimalAlliesDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<VolunteerId>> Create(Volunteer entity, CancellationToken cancellationToken = default)
    {

        if (entity == null)
            return Result<VolunteerId>.Failure(new Error("Null argument", "Entity is null"));
        
        await _context.Volunteers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
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