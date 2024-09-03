using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.CreateRequisites;

public class CreateRequisitesHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public CreateRequisitesHandler(
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

        var requisites = request.Dto.Requisites
            .Select(x => Requisite.Create(x.Title, x.Description).Value);

        var volunteerRequisites = new VolunteerRequisites(requisites.ToList());

        volunteer.Value.UpdateRequisites(volunteerRequisites);

        var result = await _repository.Save(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated volunteer requisites",  request.Id);

        return result;
    }
}