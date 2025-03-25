using Hangfire;
using Hangfire.PostgreSql;
using NotificationService.Api.Extensions;
using NotificationService.Application.Abstraction;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.DbContext;
using NotificationService.Infrastructure.Services;
using NotificationService.Options;
using Serilog;
using EmailValidator = NotificationService.Validators.EmailValidator;

namespace NotificationService;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApp(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddExtensions(configuration)
            .AddMailConfiguration(configuration)
            .AddTelegramConfiguration()
            .AddInfrastructure();
        
        return services;
    }

    private static IServiceCollection AddMailConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MailOptions>(
            configuration.GetSection(MailOptions.SECTION_NAME));
        services.AddScoped<EmailValidator>();
        services.AddScoped<MailSenderService>();

        return services;
    }

    private static IServiceCollection AddTelegramConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<TelegramBotHttpClient>();
            
        return services;
    }

    private static IServiceCollection AddExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogger(configuration);

        services.AddHttpLogging(o =>
        {
            o.CombineLogs = true;
        });

        services.AddSerilog();

        services.AddHangfire(options =>
        {
            options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(
                    c => 
                        c.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        });

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IMigrator, Migrator>();

        return services;
    }
}