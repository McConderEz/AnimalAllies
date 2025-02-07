using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.RestoreVolunteer;

public class RestoreVolunteerHandler: ICommandHandler<RestoreVolunteerCommand, VolunteerId>
{
    private readonly ILogger<RestoreVolunteerHandler> _logger;
    private readonly IValidator<RestoreVolunteerCommand> _validator;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RestoreVolunteerHandler(
        ILogger<RestoreVolunteerHandler> logger,
        IValidator<RestoreVolunteerCommand> validator,
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<VolunteerId>> Handle(
        RestoreVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await _volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Errors;
        
        volunteer.Value.Restore();

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} has been restored", command.VolunteerId);

        return volunteerId;
    }
}