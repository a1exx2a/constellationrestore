using ConstellationRestore.Dtos;
using ConstellationRestore.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstellationRestore.Controllers;

[ApiController]
[Route("api/constellations")]
public class ConstellationsController : ControllerBase
{
    private readonly IConstellationService _service;

    public ConstellationsController(IConstellationService service)
    {
        _service = service;
    }

    [HttpPost("restore")]
    [ProducesResponseType(typeof(RestoreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RestoreResponse> Restore([FromBody] RestoreRequest request)
    {

        string missing = _service.GetMissingSymbols(request.Reference!, request.Damaged!);

        return Ok(new RestoreResponse
        {
            MissingSymbols = missing,
            IsComplete = missing.Length == 0
        });
    }
}