using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvolucionesController : ControllerBase
{
    private readonly EvolucionService _service;

    public EvolucionesController(EvolucionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var evoluciones = await _service.ObtenerTodosAsync();

        var resultado = evoluciones
            .Select(MapearEvolucion)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var evolucion = await _service.ObtenerPorIdAsync(id);

        if (evolucion == null)
        {
            return NotFound(new
            {
                mensaje = "Evolución no encontrada."
            });
        }

        return Ok(MapearEvolucion(evolucion));
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearEvolucionDto datos)
    {
        if (!await _service.ExisteHistoriaClinicaAsync(
            datos.HistoriaClinicaId))
        {
            return BadRequest(new
            {
                mensaje = "La historia clínica no existe."
            });
        }

        var evolucion = new Evolucion
        {
            HistoriaClinicaId = datos.HistoriaClinicaId,
            Fecha = DateTime.Now,
            Descripcion = datos.Descripcion
        };

        await _service.AgregarAsync(evolucion);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = evolucion.Id },
            MapearEvolucion(evolucion)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarEvolucionDto datos)
    {
        var evolucion = await _service.ObtenerPorIdAsync(id);

        if (evolucion == null)
        {
            return NotFound(new
            {
                mensaje = "Evolución no encontrada."
            });
        }

        if (!await _service.ExisteHistoriaClinicaAsync(
            datos.HistoriaClinicaId))
        {
            return BadRequest(new
            {
                mensaje = "La historia clínica no existe."
            });
        }

        evolucion.HistoriaClinicaId = datos.HistoriaClinicaId;
        evolucion.Descripcion = datos.Descripcion;

        await _service.ActualizarAsync(evolucion);

        return Ok(MapearEvolucion(evolucion));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var evolucion = await _service.ObtenerPorIdAsync(id);

        if (evolucion == null)
        {
            return NotFound(new
            {
                mensaje = "Evolución no encontrada."
            });
        }

        await _service.EliminarAsync(evolucion);

        return NoContent();
    }

    private static EvolucionDto MapearEvolucion(Evolucion evolucion)
    {
        return new EvolucionDto
        {
            Id = evolucion.Id,
            HistoriaClinicaId = evolucion.HistoriaClinicaId,
            Fecha = evolucion.Fecha,
            Descripcion = evolucion.Descripcion
        };
    }
}