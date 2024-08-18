using System.Runtime.InteropServices.JavaScript;
using AnimalAllies.Domain.Models;

namespace AnimalAllies.API.Response;

public record Envelope
{
    public object? Result { get; }
    
    public string? ErrorCode { get; }
    
    public string? ErrorMessage { get; }
    
    public DateTime TimeGenerated { get; }

    private Envelope(object? result, Error? error)
    {
        Result = result;
        ErrorCode = error?.ErrorCode;
        ErrorMessage = error?.ErrorMessage;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Error(Error error) =>
        new(null, error);
}
