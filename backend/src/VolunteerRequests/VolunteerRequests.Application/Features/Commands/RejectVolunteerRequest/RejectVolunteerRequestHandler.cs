﻿using AnimalAllies.Accounts.Contracts;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Discussion.Contracts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Application.Repository;
using VolunteerRequests.Domain.Aggregates;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Application.Features.Commands.RejectVolunteerRequest;

public class RejectVolunteerRequestHandler: ICommandHandler<RejectVolunteerRequestCommand, string>
{
    private readonly ILogger<RejectVolunteerRequestHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IValidator<RejectVolunteerRequestCommand> _validator;
    private readonly IProhibitionSendingRepository _prohibitionSendingRepository;
    private readonly IDateTimeProvider _dateTimeProvider;


    public RejectVolunteerRequestHandler(
        ILogger<RejectVolunteerRequestHandler> logger, 
        [FromKeyedServices(Constraints.Context.VolunteerRequests)]IUnitOfWork unitOfWork, 
        IVolunteerRequestsRepository repository, 
        IValidator<RejectVolunteerRequestCommand> validator,
        IProhibitionSendingRepository prohibitionSendingRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _validator = validator;
        _prohibitionSendingRepository = prohibitionSendingRepository;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<string>> Handle(
        RejectVolunteerRequestCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
            
            var volunteerRequest = await _repository.GetById(volunteerRequestId, cancellationToken);
            if (volunteerRequest.IsFailure)
                return volunteerRequest.Errors;

            if (volunteerRequest.Value.AdminId != command.AdminId)
                return Error.Failure("access.denied", 
                    "this request is under consideration by another admin");

            var prohibitionSendingId = ProhibitionSendingId.NewGuid();
            var prohibitionSending = ProhibitionSending.Create(
                prohibitionSendingId,
                volunteerRequest.Value.UserId,
                _dateTimeProvider.UtcNow);

            if (prohibitionSending.IsFailure)
                return prohibitionSending.Errors;
            
            var prohibitionSendingResult = await _prohibitionSendingRepository.Create(prohibitionSending.Value, cancellationToken);

            if (prohibitionSendingResult.IsFailure)
                return prohibitionSendingResult.Errors;

            var rejectionComment = RejectionComment.Create(command.RejectionComment).Value;

            var rejectResult = volunteerRequest.Value.RejectRequest(rejectionComment);
            if (rejectResult.IsFailure)
                return rejectResult.Errors;

            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("Volunteer request with id {volunteerRequestId} was rejected",
                command.VolunteerRequestId);

            return rejectionComment.Value;
        }
        catch (Exception e)
        {
            transaction.Rollback();

            return Error.Failure("fail.reject.request", "Fail to reject request");
        }

    }
}