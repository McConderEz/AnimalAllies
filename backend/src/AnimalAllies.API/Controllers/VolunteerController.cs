using AnimalAllies.API.Contracts;
using AnimalAllies.API.Extensions;
using AnimalAllies.API.Processors;
using AnimalAllies.API.Response;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Application.Features.Volunteer.AddPet;
using AnimalAllies.Application.Features.Volunteer.AddPetPhoto;
using AnimalAllies.Application.Features.Volunteer.CreateRequisites;
using AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;
using AnimalAllies.Application.Features.Volunteer.CreateVolunteer;
using AnimalAllies.Application.Features.Volunteer.DeleteVolunteer;
using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;
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

        var command = request.ToCommand();
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Error.ToErrorResponse();
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}/main-info")]
    public async Task<IActionResult> UpdateVolunteer(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] UpdateVolunteerHandler handler,
        [FromServices] IValidator<UpdateVolunteerCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/requisites")]
    public async Task<IActionResult> CreateRequisitesToVolunteer(
        [FromRoute] Guid id,
        [FromBody] CreateRequisitesRequest request,
        [FromServices] CreateRequisitesHandler handler,
        [FromServices] IValidator<CreateRequisitesCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
    
    [HttpPut("{id:guid}/social-networks")]
    public async Task<IActionResult> CreateSocialNetworksToVolunteer(
        [FromRoute] Guid id,
        [FromBody] CreateSocialNetworksRequest request,
        [FromServices] CreateSocialNetworksHandler handler,
        [FromServices] IValidator<CreateSocialNetworksCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(command, cancellationToken);
        
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
        [FromServices] IValidator<DeleteVolunteerCommand> validator,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        
        if (validationResult.IsValid == false)
        {
            return validationResult.ToValidationErrorResponse();
        }
        
        var response = await handler.Handle(command, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
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
        var command = new AddPetCommand(
            id,
            request.Name,
            request.PetPhysicCharacteristicsDto,
            request.PetDetailsDto,
            request.AddressDto,
            request.PhoneNumber,
            request.HelpStatus,
            request.AnimalTypeDto,
            request.RequisitesDto);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToErrorResponse();

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
            return result.Error.ToErrorResponse();
        
        return Ok(result.Value);
    }
}