using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Application.Repositories;

public interface ISpeciesRepository
{
    Task<Result<SpeciesId>> Create(Species entity, CancellationToken cancellationToken = default);
    Result<SpeciesId> Delete(Species entity, CancellationToken cancellationToken = default);
    Result<SpeciesId> Save(Species entity, CancellationToken cancellationToken = default);
    Task<Result<Species>> GetById(SpeciesId id, CancellationToken cancellationToken = default);
    Task<Result<List<Species>>> Get(CancellationToken cancellationToken = default);
}