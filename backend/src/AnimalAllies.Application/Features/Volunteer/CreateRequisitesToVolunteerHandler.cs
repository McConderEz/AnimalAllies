using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateRequisitesToVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public CreateRequisitesToVolunteerHandler(
        IVolunteerRepository repository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateRequisitesRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id));

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Title, x.Description).Value);

        var volunteerRequisites = new VolunteerRequisites(requisites);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated volunteer requisites",  request.Id);
        
        return await _repository.AddRequisites(VolunteerId.Create(request.Id), volunteerRequisites, cancellationToken);
    }
}