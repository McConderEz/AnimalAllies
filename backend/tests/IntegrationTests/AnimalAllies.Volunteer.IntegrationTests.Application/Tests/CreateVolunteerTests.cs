using AnimalAllies.Core.Abstractions;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CreateVolunteer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Volunteer.IntegrationTests.Application.Tests;

public class CreateVolunteerTests : VolunteerTestsBase
{
    private ICommandHandler<CreateVolunteerCommand, VolunteerId> _sut;
    
    public CreateVolunteerTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateVolunteerCommand, VolunteerId>>();
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
        
        var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == result.Value);
        
        volunteer.Should().NotBeNull();
        volunteer.Id.Should().Be(result.Value);
    }
}