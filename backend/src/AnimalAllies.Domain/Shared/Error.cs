using System.Runtime.InteropServices.JavaScript;

namespace AnimalAllies.Domain.Models;

public class Error
{
    private const string SEPARATOR = "||";
    
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    
    public string ErrorCode { get; } 
    public string ErrorMessage { get; }
    public ErrorType Type { get; }

    private Error(string errorCode, string errorMessage, ErrorType type)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Type = type;
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