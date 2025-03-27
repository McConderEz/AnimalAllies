namespace AnimalAllies.SharedKernel.Exceptions;

public class AccountBannedException: Exception
{
    public string? Message { get; }

    public AccountBannedException(string? message) : base(message)
    {
        Message = message;
    }
}