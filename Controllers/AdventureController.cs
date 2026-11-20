using GaNe.Models.Dto;
using GaNe.Services;
using Microsoft.AspNetCore.Mvc;

namespace GaNe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdventureController(IAdventurerService adventurerService) : Controller
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AdventurerDto request, CancellationToken cancellationToken)
    {
        var req = await adventurerService.CreateAdventurer(request, cancellationToken);
        return Ok(req);
    }

    [HttpGet("all")]
    public async Task<List<AdventurerDto>> All(CancellationToken cancellationToken)
    {
        var res = await adventurerService.GetAdventurers(cancellationToken);
        return res;
    }
}