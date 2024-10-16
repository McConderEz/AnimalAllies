using AnimalAllies.Accounts.Application.AccountManagement.Commands.Login;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;
using AnimalAllies.Accounts.Contracts.Requests;
using AnimalAllies.Framework;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.Accounts.Presentation;

public class AccountController: ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request, 
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.UserName,
            request.FullNameDto,
            request.SocialNetworkDtos,
            request.Password);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Errors.ToResponse();
        
        return Ok(result.IsSuccess);
    } 
    
    [HttpPost("authentication")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request, 
        [FromServices] LoginUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Errors.ToResponse();
        
        return Ok(result.Value);
    } 
    
}