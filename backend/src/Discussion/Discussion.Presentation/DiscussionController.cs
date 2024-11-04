using AnimalAllies.Core.Models;
using AnimalAllies.Framework;
using AnimalAllies.Framework.Authorization;
using AnimalAllies.SharedKernel.Shared;
using Discussion.Application.Features.Commands.CloseDiscussion;
using Discussion.Application.Features.Commands.DeleteMessage;
using Discussion.Application.Features.Commands.PostMessage;
using Discussion.Application.Features.Commands.UpdateMessage;
using Discussion.Application.Features.Queries;
using Discussion.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Discussion.Presentation;

public class DiscussionController: ApplicationController
{
    [Permission("discussion.create")]
    [HttpPost("posting-message")]
    public async Task<IActionResult> PostMessage(
        [FromBody] PostMessageRequest request,
        [FromServices] PostMessageHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new PostMessageCommand(request.DiscussionId, userId, request.Text);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("discussion.delete")]
    [HttpPost("deletion-message")]
    public async Task<IActionResult> DeleteMessage(
        [FromBody] DeleteMessageRequest request,
        [FromServices] DeleteMessageHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new DeleteMessageCommand(request.DiscussionId, userId, request.MessageId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("discussion.update")]
    [HttpPut("editing-message")]
    public async Task<IActionResult> UpdateMessage(
        [FromBody] UpdateMessageRequest request,
        [FromServices] UpdateMessageHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new UpdateMessageCommand(request.DiscussionId, userId, request.MessageId, request.Text);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("discussion.update")]
    [HttpPut("{discussionId:guid}/closing-discussion")]
    public async Task<IActionResult> CloseDiscussion(
        [FromRoute] Guid discussionId,
        [FromServices] CloseDiscussionHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userIdString = HttpContext.User.Claims.FirstOrDefault(u => u.Type == CustomClaims.Id)?.Value;
        if (userIdString is null)
            return Error.Null("user.id.null", "user id is null").ToResponse();

        if (!Guid.TryParse(userIdString, out var userId))
            return Error.Failure("parse.error", "cannot convert user id to guid").ToResponse();
        
        var command = new CloseDiscussionCommand(discussionId, userId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
    
    [Permission("discussion.read")]
    [HttpGet("messages-by-relation-id")]
    public async Task<IActionResult> GetMessagesByRelationId(
        [FromQuery] GetMessagesByRelationIdRequest request,
        [FromServices] GetDiscussionByRelationIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        
        var query = new GetDiscussionByRelationIdQuery(request.RelationId, request.PageSize);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
}