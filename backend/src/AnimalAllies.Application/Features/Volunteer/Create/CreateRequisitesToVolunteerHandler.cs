using AnimalAllies.Application.Features.Volunteer.Update;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public class CreateRequisitesToVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<CreateRequisitesToVolunteerHandler> _logger;

    public CreateRequisitesToVolunteerHandler(
        IVolunteerRepository repository,
        ILogger<CreateRequisitesToVolunteerHandler> logger)
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

        var volunteerRequisites = new VolunteerRequisites(requisites);

        volunteer.Value.UpdateRequisites(volunteerRequisites);

        var result = await _repository.Update(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated volunteer requisites",  request.Id);

        return result;
    }
}