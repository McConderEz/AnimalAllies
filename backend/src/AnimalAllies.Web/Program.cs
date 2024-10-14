using AnimalAllies.Accounts.Application;
using AnimalAllies.Accounts.Infrastructure;
using AnimalAllies.Web.Extensions;
using AnimalAllies.Web.Middlewares;
using AnimalAllies.Species.Application;
using AnimalAllies.Volunteer.Application;
using AnimalAllies.Volunteer.Infrastructure;
using Serilog;
using AnimalAllies.Species.Infrastructure;
using AnimalAllies.Species.Presentation;
using AnimalAllies.Volunteer.Presentation;


var builder = WebApplication.CreateBuilder(args);

//TODO: Вынести добавление всех сервисов в отдельный Extensions класс

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
});

builder.Services.AddSerilog();
builder.Services
    .AddAccountsApplication()
    .AddAccountsInfrastructure(builder.Configuration)
    .AddVolunteerPresentation()
    .AddVolunteerApplication()
    .AddVolunteerInfrastructure(builder.Configuration)
    .AddSpeciesPresentation()
    .AddSpeciesApplication()
    .AddSpeciesInfrastructure(builder.Configuration);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

var app = builder.Build();

var acountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await acountsSeeder.SeedAsync();

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