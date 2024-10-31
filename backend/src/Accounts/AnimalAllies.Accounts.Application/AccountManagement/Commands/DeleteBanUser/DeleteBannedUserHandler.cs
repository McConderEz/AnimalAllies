using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.DeleteBanUser;

public class DeleteBannedUserHandler: ICommandHandler<DeleteBannedUserCommand>
{
    private readonly ILogger<DeleteBannedUserHandler> _logger;
    private readonly IValidator<DeleteBannedUserCommand> _validator;
    private readonly IBanManager _banManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteBannedUserHandler(
        ILogger<DeleteBannedUserHandler> logger,
        IValidator<DeleteBannedUserCommand> validator,
        IBanManager banManager,
        IDateTimeProvider dateTimeProvider,
        [FromKeyedServices(Constraints.Context.Accounts)]IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _banManager = banManager;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBannedUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var bannedUser = await _banManager.GetBannedUserById(command.UserId, cancellationToken);
        if (bannedUser.IsFailure || bannedUser.Value is null)
            return Errors.General.NotFound(command.UserId);
        
        _banManager.DeleteBannedUser(bannedUser.Value);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("deleted banned user with id {id}", bannedUser.Value.Id.Id);

        return Result.Success();
    }

    public async Task<Result> Handle(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Handle(new DeleteBannedUserCommand(userId), cancellationToken);
    }
}