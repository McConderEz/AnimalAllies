using Telegram.Bot;
using TelegramBotService;
using TelegramBotService.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.ConfigureTelegramBotService(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var bot =  scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

    var webhookInfo = await bot.GetWebhookInfo();
    Console.WriteLine($"Current webhook URL: {webhookInfo.Url}");
    
    await bot.SetWebhook("https://204f-141-95-145-2.ngrok-free.app/api/Account");
}

app.Run();
