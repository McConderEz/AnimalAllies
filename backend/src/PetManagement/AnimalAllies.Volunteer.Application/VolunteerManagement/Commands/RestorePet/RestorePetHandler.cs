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

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.RestorePet;

public class RestorePetHandler: ICommandHandler<RestorePetCommand, PetId>
{
    private readonly ILogger<RestorePetHandler> _logger;
    private readonly IValidator<RestorePetCommand> _validator;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RestorePetHandler(
        ILogger<RestorePetHandler> logger,
        IValidator<RestorePetCommand> validator,
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<PetId>> Handle(
        RestorePetCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await _volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Errors;
        
        var petId = PetId.Create(command.PetId);
        volunteer.Value.RestorePet(petId);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("pet with id {petId} has been restored", command.PetId);

        return petId;
    }
}