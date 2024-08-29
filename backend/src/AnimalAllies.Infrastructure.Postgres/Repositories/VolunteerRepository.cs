using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using Microsoft.EntityFrameworkCore;


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
        await _context.Volunteers.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
        
    }

    public async Task<Result<VolunteerId>> Delete(Volunteer entity, CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<Result<VolunteerId>> Save(Volunteer entity, CancellationToken cancellationToken = default)
    {
        _context.Volunteers.Attach(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
    
    public async Task<Result<Volunteer>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Include(x => x.Pets)
            .FirstOrDefaultAsync(x => x.Id == id,cancellationToken);

        if (volunteer == null)
            return Result<Volunteer>.Failure(Errors.General.NotFound(id.Id));

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task<Result<Volunteer>> GetByPhoneNumber(
        PhoneNumber phone, CancellationToken cancellationToken = default)
    {
        
        var volunteer = await _context.Volunteers
            .Include(x => x.Pets)
            .FirstOrDefaultAsync(x => x.Phone == phone, cancellationToken);

        if (volunteer == null)
            return Result<Volunteer>.Failure(Errors.General.NotFound());

        return Result<Volunteer>.Success(volunteer);
        
    }

    public async Task<Result<Volunteer>> GetByEmail(Email email, CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Include(x => x.Pets)
            .FirstOrDefaultAsync(x => x.Email == email,cancellationToken);

        if (volunteer == null)
            return Result<Volunteer>.Failure(Errors.General.NotFound());

        return Result<Volunteer>.Success(volunteer);
    }

    public async Task<Result<List<Volunteer>>> Get(CancellationToken cancellationToken = default)
    {
        var volunteers = await _context.Volunteers.ToListAsync(cancellationToken);

        return Result<List<Volunteer>>.Success(volunteers);
    }
}