using AnimalAllies.Accounts.Application.Extensions;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IValidator<RegisterUserCommand> _validator;
    
    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        IValidator<RegisterUserCommand> validator,
        RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _roleManager = roleManager;
    }
    
    public async Task<Result> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Admin",cancellationToken);
        if (role is null)
            return Errors.General.NotFound();

        var user = User.CreateAdmin(command.UserName, command.Email, role);
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User created:{name} a new account with password", command.UserName);
            return Result.Success();
        }
        
        return result.Errors.ToErrorList();
    }
}