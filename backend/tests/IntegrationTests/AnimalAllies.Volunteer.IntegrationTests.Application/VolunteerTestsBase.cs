using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace AnimalAllies.Volunteer.IntegrationTests.Application;

public class VolunteerTestsBase: IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory _factory;
    protected readonly IServiceScope _scope;
    protected readonly WriteDbContext _context;
    protected readonly Fixture _fixture;

    protected VolunteerTestsBase(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _scope = factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _fixture = new Fixture();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
        
        _scope.Dispose();
    }
}