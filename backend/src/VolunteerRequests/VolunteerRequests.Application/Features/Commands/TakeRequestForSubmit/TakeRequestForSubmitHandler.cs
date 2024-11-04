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


namespace VolunteerRequests.Application.Features.Commands.TakeRequestForSubmit;

public class TakeRequestForSubmitHandler: ICommandHandler<TakeRequestForSubmitCommand>
{
    private readonly ILogger<TakeRequestForSubmitHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteerRequestsRepository _repository;
    private readonly IDiscussionContract _discussionContract;
    private readonly IValidator<TakeRequestForSubmitCommand> _validator;


    public TakeRequestForSubmitHandler(
        ILogger<TakeRequestForSubmitHandler> logger, 
        [FromKeyedServices(Constraints.Context.VolunteerRequests)]IUnitOfWork unitOfWork, 
        IVolunteerRequestsRepository repository, 
        IDiscussionContract discussionContract, 
        IValidator<TakeRequestForSubmitCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _discussionContract = discussionContract;
        _validator = validator;
    }

    public async Task<Result> Handle(
        TakeRequestForSubmitCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var volunteerRequestId = VolunteerRequestId.Create(command.VolunteerRequestId);
            
            var volunteerRequest = await _repository.GetById(volunteerRequestId, cancellationToken);
            if (volunteerRequest.IsFailure)
                return volunteerRequest.Errors;

            var discussionId = await _discussionContract.CreateDiscussionHandler(
                command.AdminId,
                volunteerRequest.Value.UserId,
                command.VolunteerRequestId,
                cancellationToken);

            if (discussionId.IsFailure)
                return discussionId.Errors;

            var result = volunteerRequest.Value.TakeRequestForSubmit(command.AdminId, discussionId.Value.Id);
            if (result.IsFailure)
                return result.Errors;

            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
            
            _logger.LogInformation("admin with id {id} take volunteer request for submit with id {volunteerRequestId}",
                command.AdminId, command.VolunteerRequestId);

            return Result.Success();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            
            _logger.LogError("Cannot take request for submit");
            
            return Error.Failure("take.request.for.submit.failure",
                "Something went wrong with take request for submit");
        }
    }
}