namespace AnimalAllies.Application.Contracts.DTOs.Volunteer;

public record class CreateVolunteerRequest(
    string firstName,
    string secondName,
    string patronymic,
    string description,
    int workExperience,
    int petsNeedHelp,
    int petsSearchingHome,
    int petsFoundHome,
    string phoneNumber);
