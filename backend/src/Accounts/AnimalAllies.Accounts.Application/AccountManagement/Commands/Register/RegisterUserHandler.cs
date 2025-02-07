using AnimalAllies.Accounts.Application.Extensions;
using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAccountManager _accountManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterUserCommand> _validator;
    
    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        IValidator<RegisterUserCommand> validator,
        RoleManager<Role> roleManager, 
        IAccountManager accountManager,
        [FromKeyedServices(Constraints.Context.Accounts)]IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _roleManager = roleManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {

            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Name == ParticipantAccount.Participant, cancellationToken);
            if (role is null)
                return Errors.General.NotFound();

            var user = User.CreateParticipant(command.UserName, command.Email, role);

            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
                Error.Failure("cannot.create.user","Can not create user");
            
            var fullName = FullName.Create(
                command.FullNameDto.FirstName,
                command.FullNameDto.SecondName,
                command.FullNameDto.Patronymic).Value;

            var participantAccount = new ParticipantAccount(fullName, user);
            
            await _accountManager.CreateParticipantAccount(participantAccount, cancellationToken);

            user.ParticipantAccount = participantAccount;
            user.ParticipantAccountId = participantAccount.Id;

            await _unitOfWork.SaveChanges(cancellationToken);
            
            transaction.Commit();
                
            _logger.LogInformation("User created:{name} a new account with password", command.UserName);

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError("Registration of user fall with error");
            
            transaction.Rollback();
            
            return Error.Failure("cannot.create.user","Can not create user");
        }
    }
}