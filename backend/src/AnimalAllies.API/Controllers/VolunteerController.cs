using AnimalAllies.Application.Abstractions;
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
        await _volunteerService.Create(request);

        return Ok();
    }
}