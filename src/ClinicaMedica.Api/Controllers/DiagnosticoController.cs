
using ClinicaMedica.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/diagnostico")]
public class DiagnosticoController : ControllerBase
{
    private readonly ClinicaMedicaContext _context;

    public DiagnosticoController(ClinicaMedicaContext context)
    {
        _context = context;
    }

    [HttpGet("conexion")]
    public async Task<IActionResult> ComprobarConexion()
    {
    try
    {
        await _context.Database.OpenConnectionAsync();
        await _context.Database.CloseConnectionAsync();

        return Ok(new
        {
            mensaje = "Conexion con MySQL exitosa"
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR MYSQL: {ex}");

        return StatusCode(503, new
        {
            mensaje = "Error al intentar conectar con MySQL"
        });
    }
}
}
