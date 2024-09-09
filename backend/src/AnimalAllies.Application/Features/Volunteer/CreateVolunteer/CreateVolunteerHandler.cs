using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteerRepository repository,
        ILogger<CreateVolunteerHandler> logger,
        IValidator<CreateVolunteerCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;
        var email = Email.Create(request.Email).Value;
        
        var volunteerByPhoneNumber = await _repository.GetByPhoneNumber(phoneNumber,cancellationToken);
        var volunteerByEmail = await _repository.GetByEmail(email,cancellationToken);

        if (!volunteerByPhoneNumber.IsFailure || !volunteerByEmail.IsFailure)
            return Errors.Volunteer.AlreadyExist();
        
        var fullName = FullName.Create(request.FullName.FirstName, request.FullName.SecondName, request.FullName.Patronymic).Value;
        var description = VolunteerDescription.Create(request.Description).Value;
        var workExperience = WorkExperience.Create(request.WorkExperience).Value;
        

        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value);
        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Title, x.Description).Value);

        
        var volunteerSocialNetworks = new ValueObjectList<SocialNetwork>(socialNetworks.ToList());
        var volunteerRequisites = new ValueObjectList<Requisite>(requisites.ToList());
        
        var volunteerId = VolunteerId.NewGuid();
        
        var volunteerEntity = new Domain.Models.Volunteer.Volunteer(
            volunteerId,
            fullName,
            email,
            description,
            workExperience,
            phoneNumber,
            volunteerSocialNetworks,
            volunteerRequisites);
        
        var result = await _repository.Create(volunteerEntity, cancellationToken);
        
        _logger.LogInformation("Created volunteer {fullName} with id {volunteerId}", fullName, volunteerId.Id);

        return result;
    }
}