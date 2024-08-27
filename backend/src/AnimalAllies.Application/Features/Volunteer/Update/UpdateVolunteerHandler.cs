using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Update;

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
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id),cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();
        
        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;
        var email = Email.Create(request.Dto.Email).Value;
        
        var volunteerByPhoneNumber = await _repository.GetByPhoneNumber(phoneNumber,cancellationToken);
        var volunteerByEmail = await _repository.GetByEmail(email,cancellationToken);

        if (!volunteerByPhoneNumber.IsFailure || !volunteerByEmail.IsFailure)
            return Errors.Volunteer.AlreadyExist();
        
        var fullName = FullName.Create(
            request.Dto.FullName.FirstName,
            request.Dto.FullName.SecondName,
            request.Dto.FullName.Patronymic).Value;
        var description = VolunteerDescription.Create(request.Dto.Description).Value;
        var workExperience = WorkExperience.Create(request.Dto.WorkExperience).Value;


        volunteer.Value.UpdateInfo(
            fullName,
            email,
            phoneNumber,
            description,
            workExperience);
        
        var result = await _repository.Update(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with title {fullName} and id {volunteerId} updated ", fullName, request.Id);

        return result;
    }
}