using System.Runtime.InteropServices.JavaScript;
using AnimalAllies.Domain.Shared;
using FluentValidation.Results;

namespace AnimalAllies.Application.Extension;

public static class ValidationExtension
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var erros = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Error.Validation(error.ErrorCode, error.ErrorMessage, validationError.PropertyName);

        return new ErrorList(erros);
    }
}