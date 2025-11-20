using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HP.Clima.API.Controllers;

/// <summary>
/// Endpoints para consulta e cadastro de informações de CEP
/// </summary>
[ApiController]
[Route("api/cep")]
[Produces("application/json")]
public class CepController(ICepService cepService) : ControllerBase
{
    /// <summary>
    /// Consulta informações de um CEP previamente cadastrado
    /// </summary>
    /// <param name="zipCode">CEP a ser consultado (formato: XXXXX-XXX ou XXXXXXXX)</param>
    /// <returns>Informações completas do CEP</returns>
    /// <response code="200">CEP encontrado com sucesso</response>
    /// <response code="400">CEP inválido ou formato incorreto</response>
    /// <response code="404">CEP não encontrado na base de dados</response>
    /// <response code="500">Erro interno do servidor</response>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/cep/01311-000
    ///     
    /// </remarks>
    [HttpGet("{zipCode}")]
    [ProducesResponseType(typeof(ZipCodeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCepInfo([FromRoute] string zipCode)
    {
        var result = await cepService.GetCepInfoAsync(zipCode);
        return Ok(result);
    }

    /// <summary>
    /// Cadastra um novo CEP consultando APIs externas (BrasilAPI ou ViaCEP)
    /// </summary>
    /// <param name="cepRequest">Dados do CEP a ser cadastrado</param>
    /// <returns>Informações completas do CEP cadastrado</returns>
    /// <response code="201">CEP cadastrado com sucesso</response>
    /// <response code="400">Dados inválidos ou formato incorreto</response>
    /// <response code="404">CEP não encontrado nas APIs externas</response>
    /// <response code="409">CEP já cadastrado anteriormente</response>
    /// <response code="500">Erro interno do servidor</response>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/cep
    ///     {
    ///        "zipCode": "01311-000"
    ///     }
    ///     
    /// Exemplo de resposta (201 Created):
    /// 
    ///     {
    ///        "zipCode": "01311000",
    ///        "street": "Avenida Paulista",
    ///        "district": "Bela Vista",
    ///        "city": "São Paulo",
    ///        "state": "SP",
    ///        "ibge": "3550308",
    ///        "location": {
    ///          "lat": -23.5613,
    ///          "lon": -46.6565
    ///        },
    ///        "provider": "brasilapi"
    ///     }
    ///     
    /// </remarks>
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