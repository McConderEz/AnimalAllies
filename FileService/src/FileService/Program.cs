using Amazon.S3;
using FileService;
using FileService.Api;
using FileService.Api.Extensions;
using FileService.Api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddHttpLogging(o =>
{
    o.CombineLogs = true;
});

builder.Services.AddSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFileServiceInfrastructure(builder.Configuration);

//TODO: достать из конфигурации minioOptions 
builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = "http://localhost:9009",
        ForcePathStyle = true,
        UseHttp = true
    };

    return new AmazonS3Client("minioadmin", "minioadmin", config);
});

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterEndpoints();

app.Run();

namespace FileService
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}