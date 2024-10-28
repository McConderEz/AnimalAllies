using AnimalAllies.Framework;
using Microsoft.AspNetCore.Mvc;

namespace VolunteerRequests.Presentation;

public class VolunteerRequestsController: ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}