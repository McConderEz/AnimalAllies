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
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    { 
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid == false)
        {
            var validationErrors = result.Errors;

            var errors = from validationError in validationErrors
                let error = Error.Validation(validationError.ErrorCode, validationError.ErrorMessage)
                select new ResponseError(error.ErrorCode, error.ErrorMessage, validationError.PropertyName);

            var envelope = Envelope.Error(errors);

            return BadRequest(envelope);
        }
        
        var response = await handler.Handle(request, cancellationToken);
        
        if (response.IsFailure)
        {
            return response.Error.ToErrorResponse();
        }
        
        return Ok(response.Value);
    }
}