using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeleteVolunteer;

public class DeleteVolunteerHandler : ICommandHandler<DeleteVolunteerCommand, VolunteerId>
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVolunteerHandler(
        IVolunteerRepository repository, 
        ILogger<DeleteVolunteerHandler> logger,
        IValidator<DeleteVolunteerCommand> validator, 
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
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
        
        
        volunteer.Value.Delete();

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} deleted ", command.Id);

        return volunteer.Value.Id;
    }
}