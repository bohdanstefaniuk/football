using Football.Services.DataLoaders;
using Microsoft.AspNetCore.Mvc;

namespace Football.Controllers;

[ApiController]
[Route("api/maintenance")]
public class MaintenanceController: ControllerBase
{
    [HttpPost("load-data")]
    public async Task<IActionResult> LoadData([FromServices] InitialLoader initialLoader)
    {
        await initialLoader.LoadAsync();
        return Ok();
    }
}