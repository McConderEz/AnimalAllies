using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Models;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetVolunteersWithPaginationQuery> _validator;
    private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;

    public GetVolunteersWithPaginationHandler(
        IReadDbContext readDbContext, 
        IValidator<GetVolunteersWithPaginationQuery> validator,
        ILogger<GetVolunteersWithPaginationHandler> logger)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<VolunteerDto>>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            validationResult.ToErrorList();

        var volunteerQuery = _readDbContext.Volunteers.AsQueryable();

        var pagedList = await volunteerQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);
        
        _logger.LogInformation("Get volunteers");

        return pagedList;
    }
}