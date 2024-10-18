using AnimalAllies.Accounts.Contracts.Responses;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Login;

public class LoginUserHandler : ICommandHandler<LoginUserCommand,LoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<LoginUserHandler> _logger;

    private readonly ITokenProvider _tokenProvider;
    private readonly IValidator<LoginUserCommand> _validator;
    
    public LoginUserHandler(
        UserManager<User> userManager,
        ILogger<LoginUserHandler> logger,
        ITokenProvider tokenProvider,
        IValidator<LoginUserCommand> validator)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
        _validator = validator;
    }
    
    public async Task<Result<LoginResponse>> Handle(
        LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Errors.General.NotFound();

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!passwordConfirmed)
        {
            return Errors.User.InvalidCredentials();
        }

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, accessToken.Jti,cancellationToken);

        _logger.LogInformation("Successfully logged in");
        
        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}