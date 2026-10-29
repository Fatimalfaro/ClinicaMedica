using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagosController : ControllerBase
{
    private readonly PagoService _service;

    public PagosController(PagoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var pagos = await _service.ObtenerTodosAsync();

        var resultado = pagos.Select(p => new PagoDto
        {
            Id = p.Id,
            TurnoId = p.TurnoId,
            Monto = p.Monto,
            Estado = p.Estado.ToString(),
            PreferenciaPagoId = p.PreferenciaPagoId,
            PagoExternoId = p.PagoExternoId,
            FechaCreacion = p.FechaCreacion
        });

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var pago = await _service.ObtenerPorIdAsync(id);

        if (pago == null)
        {
            return NotFound(new
            {
                mensaje = "No se encontró el pago."
            });
        }

        var resultado = new PagoDto
        {
            Id = pago.Id,
            TurnoId = pago.TurnoId,
            Monto = pago.Monto,
            Estado = pago.Estado.ToString(),
            PreferenciaPagoId = pago.PreferenciaPagoId,
            PagoExternoId = pago.PagoExternoId,
            FechaCreacion = pago.FechaCreacion
        };

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearPagoDto dto)
    {
        var existeTurno = await _service.ExisteTurnoAsync(dto.TurnoId);

        if (!existeTurno)
        {
            return BadRequest(new
            {
                mensaje = "El turno indicado no existe."
            });
        }

        var monto = await _service.ObtenerMontoTurnoAsync(dto.TurnoId);

        if (monto == null)
        {
            return BadRequest(new
            {
                mensaje = "No se pudo obtener el precio de la consulta."
            });
        }

        var pago = new Pago
        {
            TurnoId = dto.TurnoId,
            Monto = monto.Value,
            FechaCreacion = DateTime.Now
        };

        await _service.AgregarAsync(pago);

        var resultado = new PagoDto
        {
            Id = pago.Id,
            TurnoId = pago.TurnoId,
            Monto = pago.Monto,
            Estado = pago.Estado.ToString(),
            PreferenciaPagoId = pago.PreferenciaPagoId,
            PagoExternoId = pago.PagoExternoId,
            FechaCreacion = pago.FechaCreacion
        };

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = pago.Id },
            resultado);
    }
}