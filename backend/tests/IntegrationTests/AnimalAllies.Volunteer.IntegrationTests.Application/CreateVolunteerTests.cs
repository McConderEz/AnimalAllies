using AnimalAllies.Core.Abstractions;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CreateVolunteer;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Volunteer.IntegrationTests.Application;

public class CreateVolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly IntegrationTestsWebFactory _factory;
    private readonly Fixture _fixture;
    private WriteDbContext _writeDbContext;
    private IServiceScope _scope;
    private ICommandHandler<CreateVolunteerCommand, VolunteerId> _sut;

    public CreateVolunteerTests(IntegrationTestsWebFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
    }

    private void RecreateScope()
    {
        _scope?.Dispose();
        _scope = _factory.Services.CreateScope();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateVolunteerCommand, VolunteerId>>();
    }
    
    public Task InitializeAsync()
    {
        RecreateScope();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _scope?.Dispose();
        
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Create_volunteer()
    {
        //Arrange
        var command = _fixture.CreateVolunteerCommand();
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        
        var volunteer = await _writeDbContext.Volunteers.FirstOrDefaultAsync(v => v.Id == result.Value);
        
        volunteer.Should().NotBeNull();
        volunteer.Id.Should().Be(result.Value);
    }
}