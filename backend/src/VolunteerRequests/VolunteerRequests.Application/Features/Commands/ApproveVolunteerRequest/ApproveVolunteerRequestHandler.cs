using AnimalAllies.Accounts.Contracts;
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

namespace VolunteerRequests.Application.Features.Commands.ApproveVolunteerRequest;

public class ApproveVolunteerRequestHandler: ICommandHandler<ApproveVolunteerRequestCommand>
{
    private readonly ILogger<ApproveVolunteerRequestHandler> _logger;
    private readonly IValidator<ApproveVolunteerRequestCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IAccountContract _accountContract;


    public ApproveVolunteerRequestHandler(
        ILogger<ApproveVolunteerRequestHandler> logger,
        IValidator<ApproveVolunteerRequestCommand> validator, 
        [FromKeyedServices(Constraints.Context.VolunteerRequests)]IUnitOfWork unitOfWork, 
        IVolunteerRequestsRepository repository, 
        IAccountContract accountContract)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _accountContract = accountContract;
    }

    public async Task<Result> Handle(
        ApproveVolunteerRequestCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        
        //TODO: переписать на интеграционные события через Rabbitmq
        
        try
        {
            var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
            
            var volunteerRequest = await _repository.GetById(volunteerRequestId, cancellationToken);
            if (volunteerRequest.IsFailure)
                return volunteerRequest.Errors;

            if (volunteerRequest.Value.AdminId != command.AdminId)
                return Error.Failure("access.denied", 
                    "this request is under consideration by another admin");

            var approveResult = volunteerRequest.Value.ApproveRequest();
            if (approveResult.IsFailure)
                return approveResult.Errors;


            var createVolunteerAccountResult = await _accountContract.CreateVolunteerAccount(
                volunteerRequest.Value.UserId, volunteerRequest.Value.VolunteerInfo, cancellationToken);
            if (createVolunteerAccountResult.IsFailure)
                return createVolunteerAccountResult.Errors;

            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("Approved volunteer request with id {id}", command.VolunteerRequestId);

            return Result.Success();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            
            _logger.LogError("Fail to approve volunteer request");

            return Error.Failure("fail.approve.request", "Fail to approve volunteer request");
        }
    }
}