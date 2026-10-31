using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Authorize(Policy = "MedicoOAdministrador")]

[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly TurnoService _turnoService;

    public TurnosController(TurnoService turnoService)
    {
        _turnoService = turnoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var turnos = await _turnoService.ObtenerTodosAsync();

        var resultado = turnos.Select(t => MapearTurno(t)).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var turno = await _turnoService.ObtenerPorIdAsync(id);

        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        return Ok(MapearTurno(turno));
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearTurnoDto datos)
    {
        if (!await _turnoService.ExistePacienteAsync(datos.PacienteId))
            return BadRequest(new { mensaje = "El paciente no existe." });

        if (!await _turnoService.ExisteMedicoAsync(datos.MedicoId))
            return BadRequest(new { mensaje = "El médico no existe." });

        var turno = new Turno
        {
            PacienteId = datos.PacienteId,
            MedicoId = datos.MedicoId,
            FechaHora = datos.FechaHora,
            Estado = EstadoTurno.Pendiente
        };

        await _turnoService.AgregarAsync(turno);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = turno.Id },
            MapearTurno(turno)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarTurnoDto datos)
    {
        var turno = await _turnoService.ObtenerPorIdAsync(id);

        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        if (!await _turnoService.ExistePacienteAsync(datos.PacienteId))
            return BadRequest(new { mensaje = "El paciente no existe." });

        if (!await _turnoService.ExisteMedicoAsync(datos.MedicoId))
            return BadRequest(new { mensaje = "El médico no existe." });

        turno.PacienteId = datos.PacienteId;
        turno.MedicoId = datos.MedicoId;
        turno.FechaHora = datos.FechaHora;
        turno.Estado = datos.Estado;

        await _turnoService.ActualizarAsync(turno);

        return Ok(MapearTurno(turno));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var turno = await _turnoService.ObtenerPorIdAsync(id);

        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        await _turnoService.EliminarAsync(turno);

        return NoContent();
    }

    private static TurnoDto MapearTurno(Turno turno)
    {
        return new TurnoDto
        {
            Id = turno.Id,
            PacienteId = turno.PacienteId,
            MedicoId = turno.MedicoId,
            FechaHora = turno.FechaHora,
            Estado = turno.Estado
        };
    }
}