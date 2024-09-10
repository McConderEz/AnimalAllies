using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentAssertions;

namespace TestProject.Domain;

public class VolunteerTests
{
     [Fact]
     public void Add_Pet_With_Empty_Pets_Return_Success_Result()
     {
         // arrange
         var birthDate = DateOnly.FromDateTime(DateTime.Now);
         var creationTime = DateTime.Now;
         
         var volunteer = InitVolunteer();
         
         var pet = InitPet(birthDate, creationTime);
         
    
         // act
         var result = volunteer.AddPet(pet);
    
         // assert
         var addedPetResult = volunteer.GetPetById(pet.Id);
    
         result.IsSuccess.Should().BeTrue();
         addedPetResult.IsSuccess.Should().BeTrue();
         addedPetResult.Value.Id.Should().Be(pet.Id);
         addedPetResult.Value.Position.Should().Be(Position.First);
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
            requisites,
            null);
        return pet;
    }

    private static Volunteer InitVolunteer()
    {
        var volunteerId = VolunteerId.NewGuid();
        var fullName = FullName.Create("Test","Test","Test").Value;
        var email = Email.Create("test@gmail.com").Value;
        var volunteerDescription = VolunteerDescription.Create("Test").Value;
        var workExperience = WorkExperience.Create(20).Value;
        var phoneNumber = PhoneNumber.Create("+12345678910").Value;
        var socialNetworks = new ValueObjectList<SocialNetwork>([SocialNetwork.Create("Test", "Test").Value]);
        var requisites = new ValueObjectList<Requisite>([Requisite.Create("Test", "Test").Value]);

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            email,
            volunteerDescription,
            workExperience,
            phoneNumber,
            socialNetworks,
            requisites);

        return volunteer;
    }

    [Fact]
    public void Add_Issue_With_Other_Issues_Return_Success_Result()
    {
        // arrange
        var birthDate = DateOnly.FromDateTime(DateTime.Now);
        var creationTime = DateTime.Now;
        const int petCount = 5;

        var volunteer = InitVolunteer();
    
        var pets = Enumerable.Range(1, petCount).Select(_ =>
            InitPet(birthDate,creationTime));

        var petToAdd = InitPet(birthDate, creationTime);
    
        foreach (var pet in pets)
            volunteer.AddPet(pet);
        
         // act
        var result = volunteer.AddPet(petToAdd);
    
        // assert
        var addedPetResult = volunteer.GetPetById(petToAdd.Id);
    
        var position = Position.Create(petCount + 1).Value;
    
        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(petToAdd.Id);
        addedPetResult.Value.Position.Should().Be(position);
    }
}