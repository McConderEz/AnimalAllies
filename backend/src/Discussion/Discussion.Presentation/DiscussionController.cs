using AnimalAllies.Core.Models;
using AnimalAllies.Framework;
using AnimalAllies.Framework.Authorization;
using AnimalAllies.SharedKernel.Shared;
using Discussion.Application.Features.PostMessage;
using Discussion.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Discussion.Presentation;

public class DiscussionController: ApplicationController
{
    [Permission("discussion.create")]
    [HttpPost]
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
        
        var query = new PostMessageCommand(request.DiscussionId, userId, request.Text);

        var result = await handler.Handle(query, cancellationToken);

        if (result.IsFailure)
            result.Errors.ToResponse();

        return Ok(result);
    }
}