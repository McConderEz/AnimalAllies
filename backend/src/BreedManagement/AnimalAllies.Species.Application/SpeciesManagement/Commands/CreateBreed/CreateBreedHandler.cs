using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Species.Application.Repository;
using AnimalAllies.Species.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Species.Application.SpeciesManagement.Commands.CreateBreed;

public class CreateBreedHandler : ICommandHandler<CreateBreedCommand, BreedId>
{
    private readonly ISpeciesRepository _repository;
    private readonly IValidator<CreateBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBreedHandler> _logger;

    public CreateBreedHandler(
        ISpeciesRepository repository,
        IValidator<CreateBreedCommand> validator,
        ILogger<CreateBreedHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<BreedId>> Handle(CreateBreedCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var speciesId = SpeciesId.Create(command.SpeciesId);

        var species = await _repository.GetById(speciesId, cancellationToken);
        if (species.IsFailure)
            return Errors.General.NotFound();

        var breedId = BreedId.NewGuid();
        var name = Name.Create(command.Name).Value;

        var breed = new Breed(breedId, name);

        species.Value.AddBreed(breed);
        
        _repository.Save(species.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Breed with id {breedId} created to species with id {speciesId}", breedId.Id, speciesId.Id);

        return breedId;
    }
}