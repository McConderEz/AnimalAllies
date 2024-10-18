using System.Data;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Messaging;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Volunteer.Application.FileProvider;
using AnimalAllies.Volunteer.Application.Providers;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.AddPetPhoto;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.ValueObject;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using static AnimalAllies.SharedKernel.Shared.Errors;

namespace TestProject.Application;

public class AddPhotoToPetTests
{
    private readonly Mock<IVolunteerRepository> _volunteerRepositoryMock = new();
    private readonly Mock<ILogger<AddPetPhotosHandler>> _loggerMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IDbTransaction> _dbTransactionMock = new();
    private readonly Mock<IValidator<AddPetPhotosCommand>> _validatorMock = new();
    private readonly Mock<IMessageQueue<IEnumerable<AnimalAllies.Volunteer.Application.FileProvider.FileInfo>>> _messageQueueMock = new();

    [Fact]
    public async void Handle_Should_Add_Photo_To_Pet_When_Command_Is_Valid()
    {
        //arrange
        var bucketName = "Bucket";
        var birthDate = DateOnly.FromDateTime(DateTime.Now);
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();

        var volunteer = InitVolunteer();
        volunteer = AddPetsInVolunteer(volunteer, 1, birthDate, creationTime);

        var buffer = new byte[256];
        for (var i = 0; i < 256; i++)
            buffer[i] = 1;
        
        var fileList = new List<CreateFileDto>
        {
            new CreateFileDto(new MemoryStream(buffer), "test.png"),
            new CreateFileDto(new MemoryStream(buffer), "test2.png"),
            new CreateFileDto(new MemoryStream(buffer), "test3.png"),
        };

        var command = new AddPetPhotosCommand(volunteer.Id.Id, volunteer.Pets.First().Id.Id, fileList);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>.Success(volunteer));

        var filesData = fileList.Select(f =>
            new FileData(f.Content,new AnimalAllies.Volunteer.Application.FileProvider.FileInfo(FilePath.Create(f.FileName).Value, bucketName)));
        
        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>(), ct))
            .ReturnsAsync(Result<VolunteerId>.Success(volunteer.Id));

        _unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(_dbTransactionMock.Object);
        
        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        var filePaths = filesData.Select(f => f.FileInfo.FilePath).ToList();

        var returnedFileInfos = filesData.Select(f => f.FileInfo).ToList();

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<IEnumerable<FileData>>(), ct))
            .ReturnsAsync(Result<IReadOnlyList<FilePath>>.Success(filePaths));

        _messageQueueMock.Setup(m => m.ReadAsync(ct))
            .ReturnsAsync(returnedFileInfos);
        
        _messageQueueMock.Setup(m => m.WriteAsync(returnedFileInfos,ct))
            .Returns(Task.CompletedTask);
        

        var handler = new AddPetPhotosHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);
        
        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Equals(volunteer.Id);
        volunteer.Pets.First().PetPhotoDetails!.Count.Should().Be(3);
    }
    
    [Fact]
    public async void Handle_Should_Not_Add_Photo_To_Pet_When_Upload_Files_Is_Failure()
    {
        //arrange
        var bucketName = "Bucket";
        var birthDate = DateOnly.FromDateTime(DateTime.Now);
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();

        var volunteer = InitVolunteer();
        volunteer = AddPetsInVolunteer(volunteer, 1, birthDate, creationTime);

        var buffer = new byte[256];
        for (var i = 0; i < 256; i++)
            buffer[i] = 1;
        
        var fileList = new List<CreateFileDto>
        {
            new CreateFileDto(new MemoryStream(buffer), "test.png"),
            new CreateFileDto(new MemoryStream(buffer), "test2.png"),
            new CreateFileDto(new MemoryStream(buffer), "test3.png"),
        };

        var command = new AddPetPhotosCommand(volunteer.Id.Id, volunteer.Pets.First().Id.Id, fileList);

        var error = Error.Failure("upload.files.failure", "files didn`t upload");
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>.Success(volunteer));

        var filesData = fileList.Select(f =>
            new FileData(f.Content,new AnimalAllies.Volunteer.Application.FileProvider.FileInfo(FilePath.Create(f.FileName).Value, bucketName)));
        
        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>(), ct))
            .ReturnsAsync(Result<VolunteerId>.Success(volunteer.Id));

        _unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(_dbTransactionMock.Object);
        
        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);

        var filePaths = filesData.Select(f => f.FileInfo.FilePath).ToList();
        
        var returnedFileInfos = filesData.Select(f => f.FileInfo).ToList();

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<IEnumerable<FileData>>(), ct))
            .ReturnsAsync(Result<IReadOnlyList<FilePath>>.Failure(error));
        
        _messageQueueMock.Setup(m => m.ReadAsync(ct))
            .ReturnsAsync(returnedFileInfos);
        
        _messageQueueMock.Setup(m => m.WriteAsync(returnedFileInfos,ct))
            .Returns(Task.CompletedTask);

        var handler = new AddPetPhotosHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);
        
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().ErrorCode.Should().Be("upload.files.failure");
    }
    
    [Fact]
    public async void Handle_Should_Not_Add_Photo_To_Pet_When_Volunteer_Not_Found()
    {
        //arrange
        var bucketName = "Bucket";
        var birthDate = DateOnly.FromDateTime(DateTime.Now);
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();

        var volunteer = InitVolunteer();
        volunteer = AddPetsInVolunteer(volunteer, 1, birthDate, creationTime);

        var buffer = new byte[256];
        for (var i = 0; i < 256; i++)
            buffer[i] = 1;
        
        var fileList = new List<CreateFileDto>
        {
            new CreateFileDto(new MemoryStream(buffer), "test.png"),
            new CreateFileDto(new MemoryStream(buffer), "test2.png"),
            new CreateFileDto(new MemoryStream(buffer), "test3.png"),
        };

        var command = new AddPetPhotosCommand(volunteer.Id.Id, volunteer.Pets.First().Id.Id, fileList);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        var returnedError = Errors.General.NotFound(volunteer.Id.Id);
        
        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result<AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer>.Failure(returnedError));
        

        var handler = new AddPetPhotosHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);
        
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().Should().Be(returnedError);
    }
    
    [Fact]
    public async void Handle_Should_Not_Add_Photo_To_Pet_When_Command_Is_Invalid()
    {
        //arrange
        var bucketName = "Bucket";
        var birthDate = DateOnly.FromDateTime(DateTime.Now);
        var creationTime = DateTime.Now;

        var ct = new CancellationToken();

        var volunteer = InitVolunteer();
        volunteer = AddPetsInVolunteer(volunteer, 1, birthDate, creationTime);
        
        
        var fileList = new List<CreateFileDto>
        {
            new CreateFileDto(new MemoryStream(), "test.png"),
            new CreateFileDto(new MemoryStream(), "test2.png"),
            new CreateFileDto(new MemoryStream(), "test3.png"),
        };

        
        var command = new AddPetPhotosCommand(volunteer.Id.Id, volunteer.Pets.First().Id.Id, fileList);

        var error = Errors.General.ValueIsInvalid("Content is empty").Serialize();

        var validationsFailure = new List<ValidationFailure>()
        {
            new ValidationFailure("Content", error)
        };

        var validationResult = new ValidationResult(validationsFailure);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);

        var handler = new AddPetPhotosHandler(
            _fileProviderMock.Object,
            _volunteerRepositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _messageQueueMock.Object,
            _validatorMock.Object);
        
        //act
        var result = await handler.Handle(command, ct);
        
        //assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().InvalidField.Should().Be("Content");
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
        AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer volunteer,
        int petsCount,
        DateOnly birthDate,
        DateTime creationTime)
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