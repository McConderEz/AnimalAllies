using AnimalAllies.Accounts.Application.Extensions;
using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IParticipantManager _participantManager;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IValidator<RegisterUserCommand> _validator;
    
    public RegisterUserHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserHandler> logger,
        IValidator<RegisterUserCommand> validator,
        RoleManager<Role> roleManager, 
        IParticipantManager participantManager)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _roleManager = roleManager;
        _participantManager = participantManager;
    }
    
    public async Task<Result> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Participant",cancellationToken);
        if (role is null)
            return Errors.General.NotFound();
        
        var socialNetworks = command.SocialNetworkDtos?
            .Select(s => SocialNetwork.Create(s.Title, s.Url).Value) ?? [];

        var user = User.CreateParticipant(command.UserName, command.Email, socialNetworks,  role);
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            var fullName = FullName.Create(
                command.FullNameDto.FirstName,
                command.FullNameDto.SecondName,
                command.FullNameDto.Patronymic).Value;

            var participantAccount = new ParticipantAccount(fullName, user);
            
            await _participantManager.CreateParticipantAccount(participantAccount, cancellationToken);
            
            _logger.LogInformation("User created:{name} a new account with password", command.UserName);
            
            return Result.Success();
        }
        
        return result.Errors.ToErrorList();
    }
}