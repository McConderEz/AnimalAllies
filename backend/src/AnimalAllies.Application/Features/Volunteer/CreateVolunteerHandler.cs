using AnimalAllies.Application.Common;
using AnimalAllies.Application.Contracts.DTOs.Volunteer;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;

    public CreateVolunteerHandler(IVolunteerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<VolunteerId>> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {

        var validator = new CreateVolunteerRequestValidator();
        var result = await validator.ValidateAsync(request, cancellationToken);

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        var email = Email.Create(request.Email);

        if (phoneNumber.IsFailure)
            return Errors.General.ValueIsInvalid();
        
        if (email.IsFailure)
            return Errors.General.ValueIsInvalid();

        var volunteerByPhoneNumber = await _repository.GetByPhoneNumber(phoneNumber.Value);
        var volunteerByEmail = await _repository.GetByEmail(email.Value);

        if (!volunteerByPhoneNumber.IsFailure || !volunteerByEmail.IsFailure)
            return Errors.Volunteer.AlreadyExist();
        
        if (result.Errors.Any())
            return Result<VolunteerId>.Failure(Error.Failure("Result.failure", result.ToString("\n")));
        
        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.name, x.url).Value).ToList();
        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.title, x.description).Value).ToList();
        
        var volunteerEntity = Domain.Models.Volunteer.Create(
            VolunteerId.NewGuid(),
            request.FirstName,
            request.SecondName,
            request.Patronymic,
            request.Email,
            request.Description,
            request.WorkExperience,
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
}