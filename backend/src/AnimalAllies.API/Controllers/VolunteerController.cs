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
        [FromServices] CreateRequisitesHandler handler,
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
        [FromServices] CreateSocialNetworksHandler handler,
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

        var command = new AddPetPhotosCommand(volunteerId, petId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToErrorResponse();
        
        return Ok(result.Value);
    }
}