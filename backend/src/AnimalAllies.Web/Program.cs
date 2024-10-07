using AnimalAllies.Web.Extensions;
using AnimalAllies.Web.Middlewares;
using AnimalAllies.Species.Application;
using AnimalAllies.Volunteer.Application;
using AnimalAllies.Volunteer.Infrastructure;
using Serilog;
using AnimalAllies.Species.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
});

builder.Services.AddSerilog();
builder.Services
    .AddSpeciesApplication()
    .AddSpeciesInfrastructure(builder.Configuration)
    .AddVolunteerApplication()
    .AddVolunteerInfrastructure(builder.Configuration);


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

    //await app.ApplyMigrations();
}

    

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();