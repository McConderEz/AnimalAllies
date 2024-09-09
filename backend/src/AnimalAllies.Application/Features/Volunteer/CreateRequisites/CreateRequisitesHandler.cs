using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.CreateRequisites;

public class CreateRequisitesHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;
    private readonly IValidator<CreateRequisitesCommand> _validator;

    public CreateRequisitesHandler(
        IVolunteerRepository repository,
        ILogger<UpdateVolunteerHandler> logger,
        IValidator<CreateRequisitesCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateRequisitesCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id));

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        var requisites = request.RequisiteDtos
            .Select(x => Requisite.Create(x.Title, x.Description).Value);

        var volunteerRequisites = new ValueObjectList<Requisite>(requisites.ToList());

        volunteer.Value.UpdateRequisites(volunteerRequisites);

        var result = await _repository.Save(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated volunteer requisites",  request.Id);

        return result;
    }
}