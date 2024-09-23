using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Features.Species.Commands.DeleteSpecies;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Species.Breed;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Species.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<DeleteBreedCommand, BreedId>
{
    private readonly ISpeciesRepository _repository;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly ILogger<DeleteBreedHandler> _logger;

    public DeleteBreedHandler(
        ISpeciesRepository repository, 
        IValidator<DeleteBreedCommand> validator,
        ILogger<DeleteBreedHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }
    
    
    public async Task<Result<BreedId>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var breedId = BreedId.Create(command.BreedId);
        var speciesId = SpeciesId.Create(command.SpeciesId);

        var species = await _repository.GetById(speciesId, cancellationToken);
        if (species.IsFailure)
            return Errors.General.NotFound();

        var result = species.Value.DeleteBreed(breedId);
        if (result.IsFailure)
            return result.Errors;

        await _repository.Save(species.Value, cancellationToken);

        _logger.LogInformation("Deleted breed with id {breedId} from species with id {speciesId}",breedId.Id, speciesId.Id);

        return breedId;
    }
}