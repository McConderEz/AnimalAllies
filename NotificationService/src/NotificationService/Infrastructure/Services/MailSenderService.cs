using CSharpFunctionalExtensions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Domain.Models;
using NotificationService.Options;
using EmailValidator = NotificationService.Validators.EmailValidator;

namespace NotificationService.Infrastructure.Services;

/// <summary>
/// Сервис отправления сообщений по почте
/// </summary>
public class MailSenderService
{
    private readonly MailOptions _mailOptions;
    private readonly ILogger<MailSenderService> _logger;
    private readonly EmailValidator _validator;

    public MailSenderService(
        IOptions<MailOptions> mailOptions,
        ILogger<MailSenderService> logger,
        EmailValidator validator)
    {
        _mailOptions = mailOptions.Value;
        _logger = logger;
        _validator = validator;
    }

    /// <summary>
    /// Метод отправки данных по почте
    /// </summary>
    /// <param name="mailData">данные для отправки(адресса получателей, отправитель, основная информация)</param>
    /// <returns></returns>
    public async Task<UnitResult<string>> Send(MailData mailData)
    {
        var validationResult = _validator.Execute(mailData.To);
        if (validationResult.IsFailure)
            return validationResult.Error;

        mailData.To = validationResult.Value;

        var mail = new MimeMessage();
        
        mail.From.Add(new MailboxAddress(_mailOptions.FromDisplayName, _mailOptions.From));

        foreach (var address in mailData.To)
        {
            MailboxAddress.TryParse(address, out var mailAddress);
            mail.To.Add(mailAddress!);
        }

        var body = new BodyBuilder { HtmlBody = mailData.Body };

        mail.Body = body.ToMessageBody();
        mail.Subject = mailData.Subject;

        using var client = new SmtpClient();
        
        await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port);
        await client.AuthenticateAsync(_mailOptions.UserName, _mailOptions.Password);
        await client.SendAsync(mail);

        foreach (var address in mail.To)
            _logger.LogInformation("Email successfully sended to {to}", address);

        return UnitResult.Success<string>();
    }
}