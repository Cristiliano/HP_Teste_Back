using HP.Clima.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using HP.Clima.API.DTOs;
using HP.Clima.API.Validators;
using FluentValidation;

namespace HP.Clima.API.Controllers;

[ApiController]
[Route("api")]
public class CepController(ICepService cepService, IValidator<CepRequestDto> validator) : ControllerBase
{
    [HttpGet("cep/{zipCode}")]
    public async Task<IActionResult> GetCepInfo([FromRoute] string zipCode)
    {
        var request = new CepRequestDto { ZipCode = zipCode };
        
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { 
                Property = e.PropertyName, 
                Error = e.ErrorMessage 
            }));
        }
        
        var normalizedZipCode = request.GetNormalizedZipCode();
        
        var result = await cepService.GetCepInfoAsync(normalizedZipCode);
        
        if (result == null)
        {
            return NotFound(new { Message = $"CEP {zipCode} not found." });
        }

        return Ok(result);
    }
}