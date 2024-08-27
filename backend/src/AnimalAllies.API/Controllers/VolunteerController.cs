using AnimalAllies.API.Extensions;
using AnimalAllies.API.Response;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Application.Features.Volunteer.Create;
using AnimalAllies.Application.Features.Volunteer.Delete;
using AnimalAllies.Application.Features.Volunteer.Update;
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
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/main-info")]
    public async Task<IActionResult> UpdateVolunteer(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        [FromServices] UpdateVolunteerHandler handler,
        [FromServices] IValidator<UpdateVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(request, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/requisites")]
    public async Task<IActionResult> CreateRequisitesToVolunteer(
        [FromRoute] Guid id,
        [FromBody] RequisiteListDto dto,
        [FromServices] CreateRequisitesToVolunteerHandler handler,
        [FromServices] IValidator<CreateRequisitesRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateRequisitesRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(request, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/social-networks")]
    public async Task<IActionResult> CreateSocialNetworksToVolunteer(
        [FromRoute] Guid id,
        [FromBody] SocialNetworkListDto dto,
        [FromServices] CreateSocialNetworksToVolunteerHandler handler,
        [FromServices] IValidator<CreateSocialNetworksRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateSocialNetworksRequest(id, dto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(request, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteVolunteer(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteVolunteerRequest(id);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(request, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
}