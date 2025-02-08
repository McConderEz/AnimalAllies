using AnimalAllies.Volunteer.Application.Database;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AnimalAllies.Volunteer.IntegrationTests.Application;

public class IntegrationTestsWebFactory: WebApplicationFactory<Program>,IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("animalAllies_tests")
        .WithUsername("postgres")
        .WithPassword("345890")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(WriteDbContext));
        
        var readContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(ReadDbContext));
        
        if(writeContext is not null)
            services.Remove(writeContext);
        
        if(readContext is not null)
            services.Remove(readContext);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString() }
            }!).Build();
        
        services.AddScoped<WriteDbContext>(_ => new WriteDbContext(configuration));
        services.AddScoped<IReadDbContext>(_ => new ReadDbContext(configuration));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}