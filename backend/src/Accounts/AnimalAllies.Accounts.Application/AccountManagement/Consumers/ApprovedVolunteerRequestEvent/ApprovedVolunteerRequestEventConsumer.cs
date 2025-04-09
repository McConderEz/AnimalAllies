using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Consumers.ApprovedVolunteerRequestEvent;

public class ApprovedVolunteerRequestEventConsumer:
    IConsumer<VolunteerRequests.Contracts.Messaging.ApprovedVolunteerRequestEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ApprovedVolunteerRequestEventConsumer> _logger;
    private readonly IAccountManager _accountManager;

    public ApprovedVolunteerRequestEventConsumer(
        UserManager<User> userManager,
        IAccountManager accountManager,
        ILogger<ApprovedVolunteerRequestEventConsumer> logger)
    {
        _userManager = userManager;
        _accountManager = accountManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<VolunteerRequests.Contracts.Messaging.ApprovedVolunteerRequestEvent> context)
    {
        var message = context.Message;
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == context.Message.UserId);
        if (user is null)
            throw new Exception(Errors.General.NotFound(context.Message.UserId).ErrorMessage);

        var fullName = FullName.Create(
            message.FirstName,
            message.SecondName,
            message.Patronymic).Value;
        
        var volunteer = new VolunteerAccount(fullName, message.WorkExperience, user);
        user.VolunteerAccount = volunteer;
        //TODO: без transactional outbox
        await _accountManager.CreateVolunteerAccount(volunteer);
        
        _logger.LogInformation("created volunteer account to user with id {userId}", user.Id);
    }
}

public class ApprovedVolunteerRequestEventConsumerDefinition : ConsumerDefinition<ApprovedVolunteerRequestEventConsumer>
{
    public ApprovedVolunteerRequestEventConsumerDefinition()
    {
        
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ApprovedVolunteerRequestEventConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(c =>
        {
            c.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        });
    }
}