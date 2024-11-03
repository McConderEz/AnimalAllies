using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Discussion.Application.Repository;
using Discussion.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Discussion.Application.Features.UpdateMessage;

public class UpdateMessageHandler: ICommandHandler<UpdateMessageCommand, MessageId>
{
    private readonly ILogger<UpdateMessageHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMessageCommand> _validator;
    private readonly IDiscussionRepository _repository;

    public UpdateMessageHandler(
        ILogger<UpdateMessageHandler> logger, 
        [FromKeyedServices(Constraints.Context.Discussion)]IUnitOfWork unitOfWork, 
        IValidator<UpdateMessageCommand> validator,
        IDiscussionRepository repository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<MessageId>> Handle(
        UpdateMessageCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var discussionId = DiscussionId.Create(command.DiscussionId);

        var discussion = await _repository.GetById(discussionId, cancellationToken);
        if (discussion.IsFailure)
            return discussion.Errors;

        var messageId = MessageId.Create(command.MessageId);
        var text = Text.Create(command.Text).Value;

        var result = discussion.Value.EditComment(command.UserId, messageId, text);
        if (result.IsFailure)
            return result.Errors;

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("user with id {userId} edit message with id {messageId}" +
                               " from discussion with id {discussionId}",
            command.UserId, command.MessageId, command.MessageId);

        return messageId;
    }
}