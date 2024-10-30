using AnimalAllies.Core.Models;
using AnimalAllies.Framework;
using AnimalAllies.Framework.Authorization;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.AspNetCore.Mvc;
using VolunteerRequests.Application.Features.Commands;
using VolunteerRequests.Application.Features.Commands.CreateVolunteerRequest;
using VolunteerRequests.Application.Features.Commands.TakeRequestForSubmit;
using VolunteerRequests.Contracts.Requests;

namespace VolunteerRequests.Presentation;

public class VolunteerRequestsController: ApplicationController
{
    [Permission("volunteerRequests.create")]
    [HttpPost("creation-volunteer-request")]
    public async Task<IActionResult> CreateVolunteerRequest(
        [FromBody] CreateVolunteerRequestRequest request,
        [FromServices] CreateVolunteerRequestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new CreateVolunteerRequestCommand(
            userId,
            request.FullNameDto, 
            request.Email,
            request.PhoneNumber,
            request.WorkExperience,
            request.VolunteerDescription,
            request.SocialNetworkDtos);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.review")]
    [HttpPost("{volunteerRequestId:guid}")]
    public async Task<IActionResult> TakeRequestForSubmit(
        [FromRoute] Guid volunteerRequestId,
        [FromServices] TakeRequestForSubmitHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new TakeRequestForSubmitCommand(userId, volunteerRequestId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
}