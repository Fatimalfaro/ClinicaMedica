using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Authorize(Policy = "MedicoOAdministrador")]
[Route("api/[controller]")]
public class HistoriasClinicasController : ControllerBase
{
    private readonly HistoriaClinicaService _service;

    public HistoriasClinicasController(HistoriaClinicaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var historias = await _service.ObtenerTodosAsync();

        var resultado = historias.Select(MapearHistoria).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var historia = await _service.ObtenerPorIdAsync(id);

        if (historia == null)
        {
            return NotFound(new
            {
                mensaje = "Historia clínica no encontrada."
            });
        }

        return Ok(MapearHistoria(historia));
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearHistoriaClinicaDto datos)
    {
        if (!await _service.ExistePacienteAsync(datos.PacienteId))
        {
            return BadRequest(new
            {
                mensaje = "El paciente no existe."
            });
        }

        if (!await _service.ExisteMedicoAsync(datos.MedicoId))
        {
            return BadRequest(new
            {
                mensaje = "El médico no existe."
            });
        }

        var historia = new HistoriaClinica
        {
            PacienteId = datos.PacienteId,
            MedicoId = datos.MedicoId,
            FechaCreacion = DateTime.Now
        };

        await _service.AgregarAsync(historia);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = historia.Id },
            MapearHistoria(historia)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarHistoriaClinicaDto datos)
    {
        var historia = await _service.ObtenerPorIdAsync(id);

        if (historia == null)
        {
            return NotFound(new
            {
                mensaje = "Historia clínica no encontrada."
            });
        }

        if (!await _service.ExistePacienteAsync(datos.PacienteId))
        {
            return BadRequest(new
            {
                mensaje = "El paciente no existe."
            });
        }

        if (!await _service.ExisteMedicoAsync(datos.MedicoId))
        {
            return BadRequest(new
            {
                mensaje = "El médico no existe."
            });
        }

        historia.PacienteId = datos.PacienteId;
        historia.MedicoId = datos.MedicoId;

        await _service.ActualizarAsync(historia);

        return Ok(MapearHistoria(historia));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var historia = await _service.ObtenerPorIdAsync(id);

        if (historia == null)
        {
            return NotFound(new
            {
                mensaje = "Historia clínica no encontrada."
            });
        }

        await _service.EliminarAsync(historia);

        return NoContent();
    }

    private static HistoriaClinicaDto MapearHistoria(
        HistoriaClinica historia)
    {
        return new HistoriaClinicaDto
        {
            Id = historia.Id,
            PacienteId = historia.PacienteId,
            MedicoId = historia.MedicoId,
            FechaCreacion = historia.FechaCreacion
        };
    }
}