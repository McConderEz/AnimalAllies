using AnimalAllies.Application.Abstractions.Volunteer;
using AnimalAllies.Application.Contracts.DTOs.Volunteer;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VolunteerController: ControllerBase
{
    private readonly IVolunteerService _volunteerService;

    public VolunteerController(IVolunteerService volunteerService)
    {
        _volunteerService = volunteerService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVolunteerRequest request)
    {
        if (request == null)
        {
            return BadRequest("request null");
        }

        var volunteerEntity = Volunteer.Create(
            request.firstName,
            request.secondName,
            request.patronymic,
            request.description,
            request.workExperience,
            request.petsNeedHelp,
            request.petsSearchingHome,
            request.petsFoundHome,
            request.phoneNumber,
            null,
            null,
            null);

        if (volunteerEntity.IsFailure)
        {
            return BadRequest(volunteerEntity.Error);
        }

        return Ok();
    }
}