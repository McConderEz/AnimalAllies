using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Features.Species.Commands.CreateSpecies;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Species.Commands.DeleteSpecies;

public class DeleteSpeciesHandler: ICommandHandler<DeleteSpeciesCommand, SpeciesId>
{
    private readonly ISpeciesRepository _repository;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        ISpeciesRepository repository,
        IValidator<DeleteSpeciesCommand> validator,
        ILogger<DeleteSpeciesHandler> logger, 
        IUnitOfWork unitOfWork,
        IVolunteerRepository volunteerRepository)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<SpeciesId>> Handle(DeleteSpeciesCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var speciesId = SpeciesId.Create(command.SpeciesId);

        var species = await _repository.GetById(speciesId, cancellationToken);
        if (species.IsFailure)
            return Errors.General.NotFound();

        var volunteers = await _volunteerRepository.Get(cancellationToken);
        if (volunteers.IsFailure)
            return volunteers.Errors;

        var petOfThisSpecies = volunteers.Value
            .Any(v => v.Pets.Any(p => p.AnimalType.SpeciesId == speciesId));

        if (petOfThisSpecies)
            return Errors.Species.DeleteConflict();
        
        var result =  _repository.Delete(species.Value, cancellationToken);
        if (result.IsFailure)
            return Error.Failure("delete.species.failure", "species deletion failed");

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Species with id {speciesId} has been deleted", speciesId.Id);

        return speciesId;
    }
}