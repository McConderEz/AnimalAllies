using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBotService.Options;
using TelegramBotService.Services;

namespace TelegramBotService;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureTelegramBotService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // Отключает 400 при ошибках валидации
            });
        
        
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddSingleton<UserStateServices>();
        
        services.Configure<TelegramBotOptions>(configuration.GetSection(TelegramBotOptions.BOT));
        
        services.AddScoped<ITelegramBotClient>(config =>
        {
            var options = config.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
            var bot = new TelegramBotClient(options.Token);
            return bot;
        });

        return services;
    }
}