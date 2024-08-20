using AnimalAllies.API.Response;
using AnimalAllies.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToErrorResponse(this Error error)
    {
        
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Null => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status500InternalServerError
        };

        var responseError = new ResponseError(error.ErrorCode, error.ErrorMessage, null);
        var envelope = Envelope.Error([responseError]);
        
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }
    
}