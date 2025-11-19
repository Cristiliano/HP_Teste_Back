using HP.Clima.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HP.Clima.API.Controllers;

[ApiController]
[Route("api/cep")]
public class CepController(ICepService cepService) : ControllerBase
{
    [HttpGet("{zipCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCepInfo([FromRoute] string zipCode)
    {
        var result = await cepService.GetCepInfoAsync(zipCode);
        return Ok(result);
    }
}