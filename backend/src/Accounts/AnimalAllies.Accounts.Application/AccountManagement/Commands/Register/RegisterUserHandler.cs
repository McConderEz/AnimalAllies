using AnimalAllies.Accounts.Application.Extensions;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IValidator<RegisterUserCommand> _validator;
    
    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        IValidator<RegisterUserCommand> validator)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var user = new User
        {
            Email = command.Email,
            UserName = command.UserName,
        };
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
            _logger.LogInformation("User created:{name} a new account with password", command.UserName);
            return Result.Success();
        }
        
        return result.Errors.ToErrorList();
    }
}