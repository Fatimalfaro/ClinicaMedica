
using ClinicaMedica.Application.DTOs.Autenticacion;
using ClinicaMedica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    [AllowAnonymous]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistroUsuarioDto dto)
    {
        try
        {
            var respuesta = await _authService.RegistrarAsync(dto);

            return StatusCode(201, respuesta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginUsuarioDto dto)
    {
        try
        {
            var respuesta = await _authService.LoginAsync(dto);

            return Ok(respuesta);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensaje = ex.Message });
        }
    }
}
