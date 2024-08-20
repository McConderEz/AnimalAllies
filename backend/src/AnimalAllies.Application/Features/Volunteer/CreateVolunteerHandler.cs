using System.ComponentModel;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
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
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);

        if (phoneNumber.IsFailure)
            return phoneNumber.Error;
        
        var email = Email.Create(request.Email);

        if (email.IsFailure)
            return email.Error;

        var volunteerByPhoneNumber = await _repository.GetByPhoneNumber(phoneNumber.Value);
        var volunteerByEmail = await _repository.GetByEmail(email.Value);

        if (!volunteerByPhoneNumber.IsFailure || !volunteerByEmail.IsFailure)
            return Errors.Volunteer.AlreadyExist();
        
        var fullName = FullName.Create(request.FirstName, request.SecondName, request.Patronymic);

        if (fullName.IsFailure)
            return fullName.Error;
        
        var description = VolunteerDescription.Create(request.Description);

        if (description.IsFailure)
            return description.Error;
        
        var workExperience = WorkExperience.Create(request.WorkExperience);

        if (workExperience.IsFailure)
            return workExperience.Error;

        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.title, x.url));

        if (socialNetworks.Any(x => x.IsFailure))
            return Errors.General.ValueIsInvalid();

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.title, x.description));
        
        if(requisites.Any(x => x.IsFailure))
            return Errors.General.ValueIsInvalid();
        
        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks.Select(x => x.Value));
        var volunteerRequisites = new VolunteerRequisites(requisites.Select(x => x.Value));
        
        var volunteerEntity = new Domain.Models.Volunteer.Volunteer(
            VolunteerId.NewGuid(),
            fullName.Value,
            email.Value,
            description.Value,
            workExperience.Value,
            phoneNumber.Value,
            volunteerSocialNetworks,
            volunteerRequisites);
        
        return await _repository.Create(volunteerEntity, cancellationToken);
    }
}