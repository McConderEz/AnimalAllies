using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeleteVolunteer;

public class DeleteVolunteerHandler : ICommandHandler<DeleteVolunteerCommand, VolunteerId>
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
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteer = await _repository.GetById(VolunteerId.Create(command.Id),cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();
        
        
        var result = await _repository.Delete(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} deleted ", command.Id);

        return result;
    }
}