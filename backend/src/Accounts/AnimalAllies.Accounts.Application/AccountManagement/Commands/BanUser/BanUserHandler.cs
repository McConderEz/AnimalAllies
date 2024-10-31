using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.BanUser;

public class BanUserHandler:  ICommandHandler<BanUserCommand>
{
    private readonly ILogger<BanUserHandler> _logger;
    private readonly IValidator<BanUserCommand> _validator;
    private readonly IBanManager _banManager;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;


    public BanUserHandler(
        ILogger<BanUserHandler> logger,
        IValidator<BanUserCommand> validator,
        IBanManager banManager,
        IDateTimeProvider dateTimeProvider, 
        [FromKeyedServices(Constraints.Context.Accounts)]IUnitOfWork unitOfWork,
        UserManager<User> userManager)
    {
        _logger = logger;
        _validator = validator;
        _banManager = banManager;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Result> Handle(
        BanUserCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var isUserExist = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (isUserExist is null)
            return Errors.General.NotFound(command.UserId);

        var bannedUserId = BannedUserId.NewGuid();
        
        var bannedUser = BannedUser.Create(
            bannedUserId, 
            command.UserId, 
            command.RelationId,
            _dateTimeProvider.UtcNow);
        
        if (bannedUser.IsFailure)
            return bannedUser.Errors;

        await _banManager.BanUser(bannedUser.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("user with id {id} add to ban", command.UserId);

        return Result.Success();
    }

    public async Task<Result> Handle(Guid userId, Guid relationId, CancellationToken cancellationToken = default)
    {
        return await Handle(new BanUserCommand(userId, relationId), cancellationToken);
    }
    
}