using AnimalAllies.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.Accounts.Presentation;

[Authorize]
public class TestController: ApplicationController
{
    [HttpPost]
    public IActionResult Test()
    {
        return Ok();
    }
}