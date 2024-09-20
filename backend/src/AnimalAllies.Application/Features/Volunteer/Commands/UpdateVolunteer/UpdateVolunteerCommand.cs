using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdateVolunteer;

public record UpdateVolunteerCommand(Guid Id, UpdateVolunteerMainInfoDto Dto) : ICommand;