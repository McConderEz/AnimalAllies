using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}