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
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;
        var email = Email.Create(request.Email).Value;

        var volunteerByPhoneNumber = await _repository.GetByPhoneNumber(phoneNumber);
        var volunteerByEmail = await _repository.GetByEmail(email);

        if (!volunteerByPhoneNumber.IsFailure || !volunteerByEmail.IsFailure)
            return Errors.Volunteer.AlreadyExist();
        
        var fullName = FullName.Create(request.FirstName, request.SecondName, request.Patronymic).Value;
        var description = VolunteerDescription.Create(request.Description).Value;
        var workExperience = WorkExperience.Create(request.WorkExperience).Value;
        
        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.title, x.url).Value).ToList();
        
        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.title, x.description).Value).ToList();
        
        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);
        var volunteerRequisites = new VolunteerRequisites(requisites);
        
        var volunteerEntity = new Domain.Models.Volunteer.Volunteer(
            VolunteerId.NewGuid(),
            fullName,
            email,
            description,
            workExperience,
            phoneNumber,
            volunteerSocialNetworks,
            volunteerRequisites);
        
        return await _repository.Create(volunteerEntity, cancellationToken);
    }
}