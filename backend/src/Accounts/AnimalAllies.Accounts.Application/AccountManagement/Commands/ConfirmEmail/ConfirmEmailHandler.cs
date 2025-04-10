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

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.ConfirmEmail;

public class ConfirmEmailHandler: ICommandHandler<ConfirmEmailCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ConfirmEmailHandler> _logger;
    private readonly IValidator<ConfirmEmailCommand> _validator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailHandler(
        UserManager<User> userManager,
        ILogger<ConfirmEmailHandler> logger,
        IValidator<ConfirmEmailCommand> validator,
        IPublishEndpoint publishEndpoint,
        [FromKeyedServices(Constraints.Context.Accounts)]IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
            return Errors.General.NotFound(command.UserId);

        var result = await _userManager.ConfirmEmailAsync(user, command.Code);
        if (result.Errors.Any())
            return result.Errors.ToErrorList();

        var message = new SetStartUserNotificationSettingsEvent(user.Id);

        await _publishEndpoint.Publish(message, cancellationToken);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("User {UserId} confirmed email.", command.UserId);

        return Result.Success();
    }
}