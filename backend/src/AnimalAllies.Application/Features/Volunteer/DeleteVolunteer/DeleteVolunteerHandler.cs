using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.DeleteVolunteer;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    
    public DeleteVolunteerHandler(
        IVolunteerRepository repository, 
        ILogger<DeleteVolunteerHandler> logger,
        IValidator<DeleteVolunteerCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        DeleteVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id),cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();
        
        
        var result = await _repository.Delete(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} deleted ", request.Id);

        return result;
    }
}