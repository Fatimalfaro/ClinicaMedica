using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecetasController : ControllerBase
{
    private readonly RecetaService _service;

    public RecetasController(RecetaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var recetas = await _service.ObtenerTodosAsync();

        var resultado = recetas
            .Select(MapearReceta)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var receta = await _service.ObtenerPorIdAsync(id);

        if (receta == null)
        {
            return NotFound(new
            {
                mensaje = "Receta no encontrada."
            });
        }

        return Ok(MapearReceta(receta));
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearRecetaDto datos)
    {
        if (!await _service.ExisteHistoriaClinicaAsync(
            datos.HistoriaClinicaId))
        {
            return BadRequest(new
            {
                mensaje = "La historia clínica no existe."
            });
        }

        var receta = new Receta
        {
            HistoriaClinicaId = datos.HistoriaClinicaId,
            FechaEmision = DateTime.Now,
            Indicaciones = datos.Indicaciones
        };

        await _service.AgregarAsync(receta);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = receta.Id },
            MapearReceta(receta)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarRecetaDto datos)
    {
        var receta = await _service.ObtenerPorIdAsync(id);

        if (receta == null)
        {
            return NotFound(new
            {
                mensaje = "Receta no encontrada."
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

        receta.HistoriaClinicaId = datos.HistoriaClinicaId;
        receta.Indicaciones = datos.Indicaciones;

        await _service.ActualizarAsync(receta);

        return Ok(MapearReceta(receta));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var receta = await _service.ObtenerPorIdAsync(id);

        if (receta == null)
        {
            return NotFound(new
            {
                mensaje = "Receta no encontrada."
            });
        }

        await _service.EliminarAsync(receta);

        return NoContent();
    }

    private static RecetaDto MapearReceta(Receta receta)
    {
        return new RecetaDto
        {
            Id = receta.Id,
            HistoriaClinicaId = receta.HistoriaClinicaId,
            FechaEmision = receta.FechaEmision,
            Indicaciones = receta.Indicaciones
        };
    }
}