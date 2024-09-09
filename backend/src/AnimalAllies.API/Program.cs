using AnimalAllies.API.Extensions;
using AnimalAllies.API.Middlewares;
using AnimalAllies.Application;
using AnimalAllies.Infrastructure;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException("Seq"))
    .Enrich.WithThreadId()
    .Enrich.WithThreadName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
});

builder.Services.AddSerilog();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

/*builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});
*/

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrations();
}

    

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();