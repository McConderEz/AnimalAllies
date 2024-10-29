using AnimalAllies.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Discussion.Presentation;

public class DiscussionController: ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}