using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Delete;

public class DeleteVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(IVolunteerRepository repository, ILogger<DeleteVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id),cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        volunteer.Value.SetIsDelete();
        
        var result = await _repository.Delete(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} deleted ", request.Id);

        return result;
    }
}