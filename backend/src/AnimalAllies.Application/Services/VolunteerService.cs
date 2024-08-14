using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Common;
using AnimalAllies.Application.Contracts.DTOs.Volunteer;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Application.Services;

public class VolunteerService: IVolunteerService
{
    private readonly IVolunteerRepository _repository;

    public VolunteerService(IVolunteerRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<Result<VolunteerId>> Create(CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {

        var validator = new CreateVolunteerRequestValidator();
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.Errors.Any())
            return Result<VolunteerId>.Failure(new Error("Invalid input", result.ToString("\n")));
        
        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.name, x.url).Value).ToList();
        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.title, x.description).Value).ToList();
        
        var volunteerEntity = Volunteer.Create(
            VolunteerId.NewGuid(),
            request.FirstName,
            request.SecondName,
            request.Patronymic,
            request.Description,
            request.WorkExperience,
            request.PetsNeedHelp,
            request.PetsSearchingHome,
            request.PetsFoundHome,
            request.PhoneNumber,
            socialNetworks,
            requisites,
            null);

        if (volunteerEntity.IsFailure)
        {
            return Result<VolunteerId>.Failure(volunteerEntity.Error!);
        }
        
        return await _repository.Create(volunteerEntity.Value, cancellationToken);
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