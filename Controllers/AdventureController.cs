using GaNe.Models.Dto;
using GaNe.Services;
using Microsoft.AspNetCore.Mvc;

namespace GaNe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdventureController(IAdventurerService adventurerService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AdventurerDto request, CancellationToken cancellationToken)
    {
        var id = await adventurerService.CreateAdventurer(request, cancellationToken);
        return Ok(id);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AdventurerDto>>> All(CancellationToken cancellationToken)
    {
        var res = await adventurerService.GetAdventurers(cancellationToken);
        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AdventurerDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var adventurer = await adventurerService.GetAdventurer(id, cancellationToken);
        if (adventurer is null)
        {
            return NotFound();
        }

        return Ok(adventurer);
    }

    [HttpPost("{id:guid}/dead")]
    public async Task<IActionResult> Dead(Guid id, CancellationToken cancellationToken)
    {
        await adventurerService.DeadAdventurer(id, cancellationToken);
        return NoContent();
    }
}
