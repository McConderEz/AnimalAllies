using System.Globalization;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.AddAvatar;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.AddSocialNetworks;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.Login;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.Refresh;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;
using AnimalAllies.Accounts.Application.AccountManagement.Queries.GetUserById;
using AnimalAllies.Accounts.Contracts.Requests;
using AnimalAllies.Core.Models;
using AnimalAllies.Framework;
using AnimalAllies.Framework.Models;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpPost("refreshing")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RefreshTokensCommand(request.AccessToken, request.RefreshToken);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost("social-networks-to-user")]
    public async Task<IActionResult> AddSocialNetworksToUser(
        [FromBody] AddSocialNetworksRequest request,
        [FromServices] UserScopedData userScopedData,
        [FromServices] AddSocialNetworkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddSocialNetworkCommand(userScopedData.UserId, request.SocialNetworkDtos);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result.IsSuccess);
    }
    
    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> AddAvatarToUser(
        [FromBody] AddAvatarRequest request,
        [FromServices] UserScopedData userScopedData,
        [FromServices] AddAvatarHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddAvatarCommand(userScopedData.UserId, request.UploadFileDto);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult> Get(
        [FromRoute] Guid userId,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(userId);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
}