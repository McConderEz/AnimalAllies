using AnimalAllies.API.Response;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApplicationController: ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);

        return base.Ok(envelope);
    }
    
    
}