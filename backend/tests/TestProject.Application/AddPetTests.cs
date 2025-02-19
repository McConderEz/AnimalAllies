using System.Runtime.InteropServices.JavaScript;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Species.Application.Database;
using AnimalAllies.Species.Application.Repository;
using AnimalAllies.Species.Contracts;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.AddPet;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject.Application;

public class AddPetTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IVolunteerRepository> _volunteerRepositoryMock = new();
    private readonly Mock<ILogger<AddPetHandler>> _loggerMock = new();
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock = new();
    private readonly Mock<IReadDbContext> _readDbContext = new();
    private readonly Mock<ISpeciesContracts> _speciesContractMock = new();
    
    
    [Fact]
    public async void Handle_Should_Add_Pet_When_Command_Is_Valid()
    {
        //arrange
        var birthDate = DateTime.Now;
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();
        
        var volunteer = InitVolunteer();
        var command = new AddPetCommand(
            volunteer.Id.Id,
            "Cat",
            new PetPhysicCharacteristicsDto(
                "White",
                "Health",
                4,
                4,
                false,
                false),
            new PetDetailsDto(
                "Desc",
                birthDate),
            new AddressDto(
                "street",
                "city",
                "state",
                "zipcode"),
            "+79494813322",
            "NeedsHelp",
            new AnimalTypeDto(Guid.NewGuid(), Guid.NewGuid()),
            new List<RequisiteDto>
            {
                new RequisiteDto{Title = "Title", Description = "Description"}
            });

        _dateTimeProviderMock.Setup(d => d.UtcNow)
            .Returns(DateTime.UtcNow);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>.Success(volunteer));
        
        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>(), ct))
            .ReturnsAsync(Result<VolunteerId>.Success(volunteer.Id));
        
        var handler = new AddPetHandler(
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _dateTimeProviderMock.Object,
            _validatorMock.Object,
            _speciesContractMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().ContainSingle();
    }
    
    [Fact]
    public async void Handle_Should_Not_Add_Pet_When_Command_Is_Invalid()
    {
        //arrange
        var birthDate = DateTime.Now;
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();

        var invalidNumber = "+7342";
        
        var volunteer = InitVolunteer();
        var command = new AddPetCommand(
            volunteer.Id.Id,
            "Cat",
            new PetPhysicCharacteristicsDto(
                "White",
                "Health",
                4,
                4,
                false,
                false),
            new PetDetailsDto(
                "Desc",
                birthDate),
            new AddressDto(
                "street",
                "city",
                "state",
                "zipcode"),
            invalidNumber,
            "NeedsHelp",
            new AnimalTypeDto(Guid.NewGuid(), Guid.NewGuid()),
            new List<RequisiteDto>
            {
                new RequisiteDto{Title = "Title", Description = "Description"}
            });

        _dateTimeProviderMock.Setup(d => d.UtcNow)
            .Returns(DateTime.UtcNow);

        var errorValidate = Errors.General.ValueIsInvalid("PhoneNumber").Serialize();

        var validationFailures = new List<ValidationFailure>
        {
            new("PhoneNumber", errorValidate)
        };

        var validationResult = new ValidationResult(validationFailures);
            
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);
        
        var handler = new AddPetHandler(
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _dateTimeProviderMock.Object,
            _validatorMock.Object,
            _speciesContractMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().InvalidField.Should().Be("PhoneNumber");
    }
    
    [Fact]
    public async void Handle_Should_Not_Add_Pet_When_Saving_Is_Error()
    {
        //arrange
        var birthDate = DateTime.Now;
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();
        
        
        var volunteer = InitVolunteer();
        var command = new AddPetCommand(
            volunteer.Id.Id,
            "Cat",
            new PetPhysicCharacteristicsDto(
                "White",
                "Health",
                4,
                4,
                false,
                false),
            new PetDetailsDto(
                "Desc",
                birthDate),
            new AddressDto(
                "street",
                "city",
                "state",
                "zipcode"),
            "+79494813322",
            "NeedsHelp",
            new AnimalTypeDto(Guid.NewGuid(), Guid.NewGuid()),
            new List<RequisiteDto>
            {
                new RequisiteDto{Title = "Title", Description = "Description"}
            });

        var error = Error.Failure("save.failure", "save method return error");
        
        _dateTimeProviderMock.Setup(d => d.UtcNow)
            .Returns(DateTime.UtcNow);
            
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>.Failure(error));
        
        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>(), ct))
            .ReturnsAsync(Result<VolunteerId>.Failure(error));
        
        var handler = new AddPetHandler(
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _dateTimeProviderMock.Object,
            _validatorMock.Object,
            _speciesContractMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);

        //assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().ErrorCode.Should().Be(error.ErrorCode);
    }

    private static Pet InitPet(DateOnly birthDate, DateTime creationTime)
    {
        var petId = PetId.NewGuid();
        var name = Name.Create("Test").Value;
        var petPhysicCharacteristic = PetPhysicCharacteristics.Create(
            "Test",
            "Test",
            1,
            1,
            false,
            false).Value;
        var petDetails = PetDetails.Create("Test", birthDate, creationTime).Value;
        var address = Address.Create(
            "Test",
            "Test",
            "Test",
            "Test").Value;
        var phoneNumber = PhoneNumber.Create("+12345678910").Value;
        var helpStatus = HelpStatus.NeedsHelp;
        var animalType = new AnimalType(SpeciesId.Empty(), Guid.Empty);
        var requisites = new ValueObjectList<Requisite>([Requisite.Create("Test", "Test").Value]);

        var pet = new Pet(
            petId,
            name,
            petPhysicCharacteristic,
            petDetails,
            address,
            phoneNumber,
            helpStatus,
            animalType,
            requisites);
        return pet;
    }

    private AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer AddPetsInVolunteer(
        AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer volunteer, int petsCount, DateOnly birthDate, DateTime creationTime)
    {
        for (var i = 0; i < petsCount; i++)
        {
            var pet = InitPet(birthDate, creationTime);
            volunteer.AddPet(pet);
        }

        return volunteer;
    }

    private static AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer InitVolunteer()
    {
        var volunteerId = VolunteerId.NewGuid();
        var fullName = FullName.Create("Test","Test","Test").Value;
        var email = Email.Create("test@gmail.com").Value;
        var volunteerDescription = VolunteerDescription.Create("Test").Value;
        var workExperience = WorkExperience.Create(20).Value;
        var phoneNumber = PhoneNumber.Create("+12345678910").Value;
        var requisites = new ValueObjectList<Requisite>([Requisite.Create("Test", "Test").Value]);

        var volunteer = new AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer(
            volunteerId,
            fullName,
            email,
            volunteerDescription,
            workExperience,
            phoneNumber,
            requisites);

        return volunteer;
    }
}