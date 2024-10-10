using AnimalAllies.Accounts.Application.AccountManagement.Commands.Login;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;
using AnimalAllies.Accounts.Presentation.Requests;
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
        var command = request.ToCommand();

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
        var command = request.ToCommand();

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Errors.ToResponse();
        
        return Ok(result.Value);
    } 
    
}