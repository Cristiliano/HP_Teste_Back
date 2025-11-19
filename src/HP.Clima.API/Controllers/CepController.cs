using HP.Clima.Domain.DTOs;
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

    [HttpPost]
    [ProducesResponseType(typeof(ZipCodeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveCepInfo([FromBody] CepRequestDto cepRequest)
    {
        var result = await cepService.SaveCepInfoAsync(cepRequest);
        
        return CreatedAtAction(
            actionName: nameof(GetCepInfo),
            routeValues: new { zipCode = result.ZipCode },
            value: result
        );
    }
}