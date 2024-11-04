using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Application.Repository;

namespace VolunteerRequests.Application.Features.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestHandler : ICommandHandler<UpdateVolunteerRequestCommand, VolunteerRequestId>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateVolunteerRequestHandler> _logger;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IValidator<UpdateVolunteerRequestCommand> _validator;

    public UpdateVolunteerRequestHandler(
        [FromKeyedServices(Constraints.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        ILogger<UpdateVolunteerRequestHandler> logger,
        IVolunteerRequestsRepository repository,
        IValidator<UpdateVolunteerRequestCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<VolunteerRequestId>> Handle(
        UpdateVolunteerRequestCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
        var volunteerRequest = await _repository.GetById(volunteerRequestId, cancellationToken);
        if (volunteerRequest.IsFailure)
            return validationResult.ToErrorList();

        if (volunteerRequest.Value.UserId != command.UserId)
            return Error.Failure("access.conflict", "Request belong another user!");
        
        var fullName = FullName.Create(
            command.FullNameDto.FirstName,
            command.FullNameDto.SecondName,
            command.FullNameDto.Patronymic).Value;

        var email = Email.Create(command.Email).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var workExperience = WorkExperience.Create(command.WorkExperience).Value;
        var volunteerDescription = VolunteerDescription.Create(command.VolunteerDescription).Value;
        var socialNetworks = command.SocialNetworkDtos
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value);

        var volunteerInfo = new VolunteerInfo(
            fullName, email, phoneNumber, workExperience, volunteerDescription, socialNetworks);

        var result = volunteerRequest.Value.UpdateVolunteerRequest(volunteerInfo);
        if (result.IsFailure)
            return result.Errors;

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("volunteer request with id {id} was updated", command.VolunteerRequestId);

        return volunteerRequestId;
    }
}