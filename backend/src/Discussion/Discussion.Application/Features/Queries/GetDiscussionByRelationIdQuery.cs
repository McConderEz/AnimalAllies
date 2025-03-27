using AnimalAllies.Core.Abstractions;

namespace Discussion.Application.Features.Queries;

public record GetDiscussionByRelationIdQuery(
    Guid RelationId,
    int PageSize) : IQuery;
