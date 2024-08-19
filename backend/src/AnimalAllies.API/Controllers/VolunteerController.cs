using AnimalAllies.API.Extensions;
using AnimalAllies.Application.Features.Volunteer;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Controllers;

public class VolunteerController: ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken token = default)
    {
        var response = await handler.Handle(request, token);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
}