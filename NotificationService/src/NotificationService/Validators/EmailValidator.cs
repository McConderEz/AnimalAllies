using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace NotificationService.Validators;

/// <summary>
/// Валидатор для проверки корректности email формата
/// </summary>
public partial class EmailValidator 
{
    private const string EMAIL_REGEX_PATTERN = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    private const string INVALID_EMAIL_ERR = "Request doesn't contain any valid reciever's adress. Aborting sending.";
    
    private readonly ILogger<EmailValidator> _logger;

    public EmailValidator(ILogger<EmailValidator> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Метод, вызывающий валидацию email-адрессов 
    /// </summary>
    /// <param name="addresses">список адрессов</param>
    /// <returns></returns>
    public Result<List<string>> Execute(List<string> addresses)
    {
        for (int i = addresses.Count - 1; i >= 0; i--)
        {
            if (EmailRegex().IsMatch(addresses[i]) == false)
            {
                _logger.LogError("Invalid email format: {mail}", addresses[i]);
                addresses.RemoveAt(i);
            }
        }

        if (addresses.Count == 0)
        {
            _logger.LogError(INVALID_EMAIL_ERR);
            return Result.Failure<List<string>>(INVALID_EMAIL_ERR);
        }

        return addresses;
    }

    [GeneratedRegex(EMAIL_REGEX_PATTERN)]
    private static partial Regex EmailRegex();
}