using AnimalAllies.API.Extensions;
using AnimalAllies.API.Response;
using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Controllers;

public class VolunteerController: ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    { 
        var response = await handler.Handle(request, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return CreatedAtAction("", response.Value);
    }
}