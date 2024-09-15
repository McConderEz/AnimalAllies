using AnimalAllies.API.Contracts;
using AnimalAllies.API.Extensions;
using AnimalAllies.API.Processors;
using AnimalAllies.Application.Features.Volunteer.AddPet;
using AnimalAllies.Application.Features.Volunteer.AddPetPhoto;
using AnimalAllies.Application.Features.Volunteer.CreateRequisites;
using AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;
using AnimalAllies.Application.Features.Volunteer.CreateVolunteer;
using AnimalAllies.Application.Features.Volunteer.DeleteVolunteer;
using AnimalAllies.Application.Features.Volunteer.MovePetPosition;
using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;
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

        var command = request.ToCommand();
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Errors.ToResponse();
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/main-info")]
    public async Task<IActionResult> UpdateVolunteer(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] UpdateVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Errors.ToResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/requisites")]
    public async Task<IActionResult> CreateRequisitesToVolunteer(
        [FromRoute] Guid id,
        [FromBody] CreateRequisitesRequest request,
        [FromServices] CreateRequisitesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Errors.ToResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/social-networks")]
    public async Task<IActionResult> CreateSocialNetworksToVolunteer(
        [FromRoute] Guid id,
        [FromBody] CreateSocialNetworksRequest request,
        [FromServices] CreateSocialNetworksHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Errors.ToResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteVolunteer(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Errors.ToResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Errors.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{volunteerId:guid}/{petId:guid}/petPhoto")]
    public async Task<ActionResult> AddPetPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] AddPetPhotosRequest request,
        [FromServices] AddPetPhotosHandler handler,
        CancellationToken cancellationToken = default)
    {
        await using var fileProcessor = new FormFileProcessor();

        var fileDtos = fileProcessor.Process(request.Files);

        var command = request.ToCommand(volunteerId, petId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Errors.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/{petId:guid}/pet-position")]
    public async Task<ActionResult> MovePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] MovePetPositionRequest request,
        [FromServices] MovePetPositionHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Errors.ToResponse();
        
        return Ok(result.Value);
    }
}