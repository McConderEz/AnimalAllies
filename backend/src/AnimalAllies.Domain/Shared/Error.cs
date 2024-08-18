namespace AnimalAllies.Domain.Models;

public class Error
{
    public string ErrorCode { get; } 
    public string ErrorMessage { get; }
    public ErrorType Type { get; }

    private Error(string errorCode, string errorMessage, ErrorType type)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Type = type;
    }

    public static Error Validation(string errorCode, string errorMessage) =>
        new Error(errorCode, errorMessage, ErrorType.Validation);
    
    public static Error Failure(string errorCode, string errorMessage) =>
        new Error(errorCode, errorMessage, ErrorType.Failure);
    
    public static Error NotFound(string errorCode, string errorMessage) =>
        new Error(errorCode, errorMessage, ErrorType.NotFound);
    
    public static Error Conflict(string errorCode, string errorMessage) =>
        new Error(errorCode, errorMessage, ErrorType.Conflict);
    
    public static Error Null(string errorCode, string errorMessage) =>
        new Error(errorCode, errorMessage, ErrorType.Null);
    
    public override string ToString()
    {
        return $"ErrorCode: {ErrorCode}.\nErrorMessage:{ErrorMessage}\n{Type}";
    }
}

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Failure,
    Null,
    Conflict
}