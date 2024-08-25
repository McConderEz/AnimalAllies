using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
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

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<VolunteerId>> Update(Volunteer entity, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers
            .Where(v => v.Id == entity.Id)
            .ExecuteUpdateAsync(v => v
                .SetProperty(x => x.FullName.FirstName, entity.FullName.FirstName)
                .SetProperty(x => x.FullName.SecondName, entity.FullName.SecondName)
                .SetProperty(x => x.FullName.Patronymic, entity.FullName.Patronymic)
                .SetProperty(x => x.Email.Value, entity.Email.Value)
                .SetProperty(x => x.Phone.Number, entity.Phone.Number)
                .SetProperty(x => x.Description.Value, entity.Description.Value)
                .SetProperty(x => x.WorkExperience.Value, entity.WorkExperience.Value),
                cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<Result<VolunteerId>> AddRequisites(
        VolunteerId id,
        VolunteerRequisites requisites, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        volunteer.UpdateRequisites(requisites);
        
        await _context.SaveChangesAsync(cancellationToken);

        return id;
    }

    public async Task<Result<VolunteerId>> AddSocialNetworks(
        VolunteerId id,
        VolunteerSocialNetworks socialNetworks,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        volunteer.UpdateSocialNetworks(socialNetworks);
        
        await _context.SaveChangesAsync(cancellationToken);

        return id;
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
        
        var volunteer = await _context.Volunteers
            .Include(x => x.Pets)
            .FirstOrDefaultAsync(x => x.Phone == phone);

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