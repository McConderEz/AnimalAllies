using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer;

public class UpdateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public UpdateVolunteerHandler(IVolunteerRepository repository, ILogger<UpdateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        UpdateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id));

        if (volunteer.IsFailure)
            return Errors.General.NotFound();
        
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;
        var email = Email.Create(request.Email).Value;
        
        var volunteerByPhoneNumber = await _repository.GetByPhoneNumber(phoneNumber);
        var volunteerByEmail = await _repository.GetByEmail(email);

        if (!volunteerByPhoneNumber.IsFailure || !volunteerByEmail.IsFailure)
            return Errors.Volunteer.AlreadyExist();
        
        var fullName = FullName.Create(
            request.FullName.FirstName,
            request.FullName.SecondName,
            request.FullName.Patronymic).Value;
        var description = VolunteerDescription.Create(request.Description).Value;
        var workExperience = WorkExperience.Create(request.WorkExperience).Value;


        volunteer.Value.UpdateInfo(
            fullName,
            email,
            phoneNumber,
            description,
            workExperience);
        
        _logger.LogInformation("volunteer with title {fullName} and id {volunteerId} updated ", fullName, request.Id);
        
        return await _repository.Update(volunteer.Value, cancellationToken);
    }
}