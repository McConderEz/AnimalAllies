using AnimalAllies.Core.Models;
using AnimalAllies.Framework;
using AnimalAllies.Framework.Authorization;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.AspNetCore.Mvc;
using VolunteerRequests.Application.Features.Commands;
using VolunteerRequests.Application.Features.Commands.ApproveVolunteerRequest;
using VolunteerRequests.Application.Features.Commands.CreateVolunteerRequest;
using VolunteerRequests.Application.Features.Commands.RejectVolunteerRequest;
using VolunteerRequests.Application.Features.Commands.SendRequestForRevision;
using VolunteerRequests.Application.Features.Commands.TakeRequestForSubmit;
using VolunteerRequests.Application.Features.Queries.GetFilteredVolunteerRequestsByAdminIdWithPagination;
using VolunteerRequests.Application.Features.Queries.GetVolunteerRequestsInWaitingWithPagination;
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
            return result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.review")]
    [HttpPost("{volunteerRequestId:guid}/taking-request-for-submitting")]
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
           return result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.review")]
    [HttpPost("sending-for-revision")]
    public async Task<IActionResult> SendRequestForRevision(
        [FromBody] SendRequestForRevisionRequest request,
        [FromServices] SendRequestForRevisionHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new SendRequestForRevisionCommand(userId, request.VolunteerRequestId, request.RejectComment);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.review")]
    [HttpPost("rejecting-request")]
    public async Task<IActionResult> RejectRequest(
        [FromBody] RejectVolunteerRequest request,
        [FromServices] RejectVolunteerRequestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new RejectVolunteerRequestCommand(userId, request.VolunteerRequestId, request.RejectComment);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.review")]
    [HttpPost("{volunteerRequestId:guid}approving-request")]
    public async Task<IActionResult> ApproveRequest(
        [FromRoute] Guid volunteerRequestId,
        [FromServices] ApproveVolunteerRequestHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new ApproveVolunteerRequestCommand(userId, volunteerRequestId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.read")]
    [HttpGet("volunteer-requests-in-waiting-with-pagination")]
    public async Task<ActionResult> GetVolunteerRequestsInWaiting(
        [FromQuery] GetVolunteerRequestsInWaitingWithPaginationRequest request,
        [FromServices] GetVolunteerRequestsInWaitingWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerRequestsInWaitingWithPaginationQuery(
            request.SortBy,
            request.SortDirection,
            request.Page, 
            request.PageSize);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("volunteerRequests.read")]
    [HttpGet("filtered-volunteer-requests-by-admin-id-with-pagination")]
    public async Task<ActionResult> GetFilteredVolunteerRequestsByAdminId(
        [FromQuery] GetFilteredRequestsByAdminIdRequest request,
        [FromServices] GetFilteredVolunteerRequestsByAdminIdWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var query = new GetFilteredVolunteerRequestsByAdminIdWithPaginationQuery(
            userId,
            request.RequestStatus,
            request.SortBy,
            request.SortDirection,
            request.Page, 
            request.PageSize);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
}