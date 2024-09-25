using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Species.Commands.CreateSpecies;

public class CreateSpeciesHandler : ICommandHandler<CreateSpeciesCommand, SpeciesId>
{
    private readonly ISpeciesRepository _repository;
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(
        ISpeciesRepository repository, 
        IValidator<CreateSpeciesCommand> validator,
        ILogger<CreateSpeciesHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<SpeciesId>> Handle(CreateSpeciesCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var speciesId = SpeciesId.NewGuid();
        var name = Name.Create(command.Name).Value;
        
        var species = new Domain.Models.Species.Species(speciesId, name);

        var result = await _repository.Create(species, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        if (result.IsFailure)
            return result.Errors;
        
        _logger.LogInformation("Created species with id {speciesId}", speciesId.Id);
        
        return result.Value;
    }
}