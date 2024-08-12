using AnimalAllies.Application.Common;
using AnimalAllies.Domain.Models;
using AnimalAllies.Infrastructure.Common;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AnimalAllies.Infrastructure.Repositories;

public class VolunteerRepository : BaseRepository,IVolunteerRepository
{
    
    public VolunteerRepository(AnimalAlliesDbContext context) : base(context)
    {
    }
    
    public async Task Create(Volunteer entity)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)} cannot be null!");
            
            await _context.Volunteers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

        }
        finally
        {
            _semaphore.Release();
        }
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