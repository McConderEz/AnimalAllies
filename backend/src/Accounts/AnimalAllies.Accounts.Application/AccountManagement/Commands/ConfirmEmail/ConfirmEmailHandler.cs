using System.Transactions;
using AnimalAllies.Accounts.Application.Extensions;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Contracts.Requests;
using Outbox.Abstractions;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.ConfirmEmail;

public class ConfirmEmailHandler: ICommandHandler<ConfirmEmailCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ConfirmEmailHandler> _logger;
    private readonly IValidator<ConfirmEmailCommand> _validator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWorkOutbox _unitOfWorkOutbox;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailHandler(
        UserManager<User> userManager,
        ILogger<ConfirmEmailHandler> logger,
        IValidator<ConfirmEmailCommand> validator,
        IPublishEndpoint publishEndpoint,
        [FromKeyedServices(Constraints.Context.Accounts)]IUnitOfWork unitOfWork,
        IOutboxRepository outboxRepository, 
        IUnitOfWorkOutbox unitOfWorkOutbox)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
        _outboxRepository = outboxRepository;
        _unitOfWorkOutbox = unitOfWorkOutbox;
    }

    public async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled
        );
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
            return Errors.General.NotFound(command.UserId);

        var result = await _userManager.ConfirmEmailAsync(user, command.Code);
        if (result.Errors.Any())
            return result.Errors.ToErrorList();

        var message = new SetStartUserNotificationSettingsEvent(user.Id);

        await _outboxRepository.AddAsync(message, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        await _unitOfWorkOutbox.SaveChanges(cancellationToken);
        
        scope.Complete();
        
        _logger.LogInformation("User {UserId} confirmed email.", command.UserId);

        return Result.Success();
    }
}