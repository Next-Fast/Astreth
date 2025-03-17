using Microsoft.AspNetCore.Mvc;

namespace Astreth.ServerPlugin.Cotrollers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AstrethCotroller(ModInfoManager manager) : ControllerBase
{
    [HttpGet]
    public IActionResult AuthMod()
    {
        return Ok();
    }
}