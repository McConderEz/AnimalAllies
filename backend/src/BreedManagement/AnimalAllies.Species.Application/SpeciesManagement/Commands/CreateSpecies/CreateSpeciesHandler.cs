using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Species.Application.Database;
using AnimalAllies.Species.Application.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Species.Application.SpeciesManagement.Commands.CreateSpecies;

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
        
        var species = new Domain.Species(speciesId, name);

        var result = await _repository.Create(species, cancellationToken);
        
        if (result.IsFailure)
            return result.Errors;
        
        _logger.LogInformation("Created species with id {speciesId}", speciesId.Id);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return result.Value;
    }
}