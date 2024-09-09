namespace AnimalAllies.Domain.Shared;

public class Error
{
    private const string SEPARATOR = "||";
    
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
    
    public string Serialize()
    {
        return string.Join(SEPARATOR, ErrorCode, ErrorMessage, Type);
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length < 3)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
        {
            throw new ArgumentException("Invalid serialized format");
        }

        return new Error(parts[0], parts[1], type);
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