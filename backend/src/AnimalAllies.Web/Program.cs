using System.Text;
using AnimalAllies.Web.Extensions;
using AnimalAllies.Web.Middlewares;
using AnimalAllies.Species.Application;
using AnimalAllies.Volunteer.Application;
using AnimalAllies.Volunteer.Infrastructure;
using Serilog;
using AnimalAllies.Species.Infrastructure;
using AnimalAllies.Species.Presentation;
using AnimalAllies.Volunteer.Presentation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
});

builder.Services.AddSerilog();
builder.Services
    .AddVolunteerPresentation()
    .AddVolunteerApplication()
    .AddVolunteerInfrastructure(builder.Configuration)
    .AddSpeciesPresentation()
    .AddSpeciesApplication()
    .AddSpeciesInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };
        
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});



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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();