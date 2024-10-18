using AnimalAllies.Accounts.Application;
using AnimalAllies.Accounts.Infrastructure;
using AnimalAllies.Accounts.Infrastructure.Seeding;
using AnimalAllies.Accounts.Presentation;
using AnimalAllies.Web.Extensions;
using AnimalAllies.Web.Middlewares;
using AnimalAllies.Species.Application;
using AnimalAllies.Volunteer.Application;
using AnimalAllies.Volunteer.Infrastructure;
using Serilog;
using AnimalAllies.Species.Infrastructure;
using AnimalAllies.Species.Presentation;
using AnimalAllies.Volunteer.Presentation;
using AnimalAllies.Web;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
});

builder.Services.AddSerilog();

builder.Services.AddModules(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await accountsSeeder.SeedAsync();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //await app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();