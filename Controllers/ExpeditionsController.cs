using Microsoft.AspNetCore.Mvc;

namespace GaNe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpeditionsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult Create([FromBody] object request)
    {
        return CreatedAtAction(nameof(GetById), new { id = Guid.Empty }, null);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] object request)
    {
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        return NoContent();
    }
}
