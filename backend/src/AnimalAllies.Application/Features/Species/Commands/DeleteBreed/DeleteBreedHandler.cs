using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Database;
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
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBreedHandler> _logger;

    public DeleteBreedHandler(
        ISpeciesRepository repository, 
        IValidator<DeleteBreedCommand> validator,
        ILogger<DeleteBreedHandler> logger,
        IUnitOfWork unitOfWork,
        IReadDbContext readDbContext)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
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

        var petOfThisBreed = _readDbContext.Pets.Any(p => p.BreedId == breedId.Id);
        
        if (petOfThisBreed)
            return Errors.Species.DeleteConflict();
        
        var result = species.Value.DeleteBreed(breedId);
        if (result.IsFailure)
            return result.Errors;

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Deleted breed with id {breedId} from species with id {speciesId}",breedId.Id, speciesId.Id);

        return breedId;
    }
}