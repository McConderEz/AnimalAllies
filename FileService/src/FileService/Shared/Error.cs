namespace FileService.Shared;

public class Error
{
    
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    
    public string ErrorCode { get; } 
    public string ErrorMessage { get; }
    public ErrorType Type { get; }
    public string? InvalidField { get; } = null;

    private Error(string errorCode, string errorMessage, ErrorType type, string? invalidField = null)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Type = type;
        InvalidField = invalidField;
    }
    

    public static Error Validation(string errorCode, string errorMessage, string? invalidField = null) =>
        new(errorCode, errorMessage, ErrorType.Validation, invalidField);
    
    public static Error Failure(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Failure);
    
    public static Error NotFound(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.NotFound);
    
    public static Error Conflict(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Conflict);
    
    public static Error Null(string errorCode, string errorMessage) =>
        new(errorCode, errorMessage, ErrorType.Null);

    public ErrorList ToErrorList() => new([this]);
    
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