using AnimalAllies.Core.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AnimalAllies.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddQuartzService()
            .AddOutbox(configuration);

        return services;
    }

    private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<OutboxContext>();

        services.AddScoped<OutboxRepository>();
        
        services.AddScoped<ProcessOutboxMessageService>();
        
        return services;
    }
    
    private static IServiceCollection AddQuartzService(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessageJob));

            configure.AddJob<ProcessOutboxMessageJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                    schedule => schedule.WithIntervalInSeconds(1).RepeatForever()));
        });
        
        services.AddQuartzHostedService(options => {options.WaitForJobsToComplete = true;});
        
        return services; 
    }
}