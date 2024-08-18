using AnimalAllies.Application.Common;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
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
            return Result<VolunteerId>.Failure(Errors.General.Null(nameof(entity)));
        
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

    public async Task<Result<Volunteer>> GetById(VolunteerId id)
    {
        var volunteer = await _context.Volunteers
            .Include(x => x.Pets)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (volunteer == null)
            return Result<Volunteer>.Failure(Errors.General.NotFound(id.Id));

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task<Result<Volunteer>> GetByPhoneNumber(PhoneNumber phone)
    {
        
        var volunteer = await _context.Volunteers.FirstOrDefaultAsync(x => x.Phone == phone);

        if (volunteer == null)
            return Result<Volunteer>.Failure(Errors.General.NotFound());

        return Result<Volunteer>.Success(volunteer);
        
    }

    public async Task<Result<Volunteer>> GetByEmail(Email email)
    {
        var volunteer = await _context.Volunteers
            .Include(x => x.Pets)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (volunteer == null)
            return Result<Volunteer>.Failure(Errors.General.NotFound());

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task<Result<List<Volunteer>>> Get()
    {
        var volunteers = await _context.Volunteers.ToListAsync();

        return Result<List<Volunteer>>.Success(volunteers);
    }
}