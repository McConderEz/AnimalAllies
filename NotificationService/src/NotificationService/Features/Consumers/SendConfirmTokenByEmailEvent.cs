using System.ComponentModel.DataAnnotations;
using System.Net;
using MailKit;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.DbContext;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;

namespace NotificationService.Features.Consumers;

public class SendConfirmTokenByEmailEvent: IConsumer<SendConfirmTokenByEmailRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly MailSenderService _mailService;
    private readonly ILogger<SendConfirmTokenByEmailEvent> _logger;
    private readonly EmailValidator _emailValidator;
    private readonly IUrlHelperFactory _urlHelperFactory;

    public SendConfirmTokenByEmailEvent(
        ApplicationDbContext dbContext,
        MailSenderService mailService,
        ILogger<SendConfirmTokenByEmailEvent> logger,
        EmailValidator emailValidator, 
        IUrlHelperFactory urlHelperFactory)
    {
        _dbContext = dbContext;
        _mailService = mailService;
        _logger = logger;
        _emailValidator = emailValidator;
        _urlHelperFactory = urlHelperFactory;
    }

    public async Task Consume(ConsumeContext<SendConfirmTokenByEmailRequest> context)
    {
        var message = context.Message;
        
        var validationResult = _emailValidator.Execute([message.Email]);
        if (validationResult.IsFailure)
            throw new Exception("incorrect email format");
        
        var encodedCode = WebUtility.UrlEncode(message.Code);
        
        var url = $"http://localhost:5299/Account/ConfirmEmail?" +
                  $"userId={message.UserId.ToString()}" +
                  $"&code={encodedCode}";


        var mailData = new MailData([message.Email], "Confirmation Email",
            $"Чтобы подтвердить аккаунта, перейдите по следующей ссылке: {url}");

        await _mailService.Send(mailData);
    }
}