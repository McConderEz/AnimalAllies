using AnimalAllies.API.Extensions;
using AnimalAllies.API.Response;
using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs.Volunteer;
using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;
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