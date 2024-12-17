using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.Ids;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Application.Repository;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Application.Features.Commands.SendRequestForRevision;

public class SendRequestForRevisionHandler: ICommandHandler<SendRequestForRevisionCommand, VolunteerRequestId>
{
    private readonly ILogger<SendRequestForRevisionHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SendRequestForRevisionCommand> _validator;
    private readonly IVolunteerRequestsRepository _repository;
    
    public SendRequestForRevisionHandler(
        ILogger<SendRequestForRevisionHandler> logger, 
        [FromKeyedServices(Constraints.Context.VolunteerRequests)]IUnitOfWork unitOfWork,
        IValidator<SendRequestForRevisionCommand> validator,
        IVolunteerRequestsRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<VolunteerRequestId>> Handle(
        SendRequestForRevisionCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);

        var volunteerRequest = await _repository.GetById(volunteerRequestId, cancellationToken);
        if (volunteerRequest.IsFailure)
            return volunteerRequest.Errors;

        if (volunteerRequest.Value.AdminId != command.AdminId)
            return Error.Failure("access.denied", 
                "this request is under consideration by another admin");
        
        var rejectComment = RejectionComment.Create(command.RejectionComment).Value;

        var result = volunteerRequest.Value.SendRequestForRevision(rejectComment);
        if (result.IsFailure)
            return result.Errors;

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("volunteer request with id {id} sent to revision", command.VolunteerRequestId);

        return volunteerRequestId;
    }
}