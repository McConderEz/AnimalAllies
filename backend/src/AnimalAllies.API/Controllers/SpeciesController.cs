﻿using AnimalAllies.API.Contracts.Volunteer;
using AnimalAllies.API.Extensions;
using AnimalAllies.Application.Features.Species.Commands.CreateBreed;
using AnimalAllies.Application.Features.Species.Commands.CreateSpecies;
using AnimalAllies.Application.Features.Species.Commands.DeleteBreed;
using AnimalAllies.Application.Features.Species.Commands.DeleteSpecies;
using AnimalAllies.Application.Features.Species.Queries.GetBreedsBySpeciesId;
using AnimalAllies.Application.Features.Species.Queries.GetSpeciesWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Controllers;

public class SpeciesController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateSpeciesHandler handler,
        [FromBody] CreateSpeciesRequest request,
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
    
    [HttpPost("{speciesId:guid}")]
    public async Task<IActionResult> CreateBreed(
        [FromServices] CreateBreedHandler handler,
        [FromRoute] Guid speciesId,
        [FromBody] CreateBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(speciesId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Errors.ToResponse();
        }
        
        return Ok(result.Value);
    }
    
    [HttpDelete("{speciesId:guid}")]
    public async Task<IActionResult> DeleteSpecies(
        [FromServices] DeleteSpeciesHandler handler,
        [FromRoute] Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpeciesCommand(speciesId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Errors.ToResponse();
        }
        
        return Ok(result.Value);
    }
    
    [HttpDelete("{speciesId:guid}/{breedId:guid}")]
    public async Task<IActionResult> DeleteBreed(
        [FromServices] DeleteBreedHandler handler,
        [FromRoute] Guid speciesId,
        [FromRoute] Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(speciesId, breedId);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Errors.ToResponse();
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSpecies(
        [FromServices] GetSpeciesWithPaginationHandlerDapper handler,
        [FromQuery] GetSpeciesWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {

        var query = request.ToQuery();
        
        var result = await handler.Handle(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Errors.ToResponse();
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("{speciesId:guid}")]
    public async Task<IActionResult> GetBreeds(
        [FromServices] GetBreedsBySpeciesIdWithPaginationHandlerDapper handler,
        [FromRoute] Guid speciesId,
        [FromQuery] GetBreedsBySpeciesIdWithPaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery(speciesId);
        
        var result = await handler.Handle(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Errors.ToResponse();
        }
        
        return Ok(result.Value);
    }
}