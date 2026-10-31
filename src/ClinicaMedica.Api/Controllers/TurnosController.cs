using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly TurnoService _turnoService;
    private readonly IUsuarioActualService _usuarioActualService;

    public TurnosController(
        TurnoService turnoService,
        IUsuarioActualService usuarioActualService)
    {
        _turnoService = turnoService;
        _usuarioActualService = usuarioActualService;
    }

    // GET: api/turnos
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var usuarioId = _usuarioActualService.ObtenerIdUsuario();
        var rol = _usuarioActualService.ObtenerRol();

        List<Turno> turnos;

        if (rol == nameof(RolUsuario.Administrador))
        {
            turnos = await _turnoService.ObtenerTodosAsync();
        }
        else if (rol == nameof(RolUsuario.Medico))
        {
            turnos = await _turnoService.ObtenerPorMedicoUsuarioAsync(usuarioId);
        }
        else if (rol == nameof(RolUsuario.Paciente))
        {
            turnos = await _turnoService.ObtenerPorPacienteUsuarioAsync(usuarioId);
        }
        else
        {
            return Forbid();
        }

        return Ok(turnos.Select(MapearTurno).ToList());
    }

    // GET: api/turnos/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuarioId = _usuarioActualService.ObtenerIdUsuario();
        var rol = _usuarioActualService.ObtenerRol();

        Turno? turno;

        if (rol == nameof(RolUsuario.Administrador))
        {
            turno = await _turnoService.ObtenerPorIdAsync(id);
        }
        else if (rol == nameof(RolUsuario.Medico))
        {
            turno = await _turnoService
                .ObtenerPorIdYMedicoUsuarioAsync(id, usuarioId);
        }
        else if (rol == nameof(RolUsuario.Paciente))
        {
            turno = await _turnoService
                .ObtenerPorIdYPacienteUsuarioAsync(id, usuarioId);
        }
        else
        {
            return Forbid();
        }

        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        return Ok(MapearTurno(turno));
    }

    // POST: api/turnos
    [HttpPost]
    [Authorize(Roles = "Paciente,Medico,Administrador")]
    public async Task<IActionResult> Crear(CrearTurnoDto datos)
    {
        var usuarioId = _usuarioActualService.ObtenerIdUsuario();
        var rol = _usuarioActualService.ObtenerRol();

        int pacienteId;

        if (rol == nameof(RolUsuario.Paciente))
        {
            // El paciente solamente puede solicitar turnos para sí mismo.
            var paciente = await _turnoService
                .ObtenerPacientePorUsuarioIdAsync(usuarioId);

            if (paciente == null)
                return BadRequest(new
                {
                    mensaje = "No se encontró un paciente asociado a este usuario."
                });

            pacienteId = paciente.Id;
        }
        else
        {
            // Médico y administrador pueden indicar el paciente.
            pacienteId = datos.PacienteId;
        }

        if (!await _turnoService.ExistePacienteAsync(pacienteId))
            return BadRequest(new { mensaje = "El paciente no existe." });

        if (!await _turnoService.ExisteMedicoAsync(datos.MedicoId))
            return BadRequest(new { mensaje = "El médico no existe." });

        var turno = new Turno
        {
            PacienteId = pacienteId,
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

    // PUT: api/turnos/5
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Medico,Administrador")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarTurnoDto datos)
    {
        var usuarioId = _usuarioActualService.ObtenerIdUsuario();
        var rol = _usuarioActualService.ObtenerRol();

        Turno? turno;

        if (rol == nameof(RolUsuario.Administrador))
        {
            turno = await _turnoService.ObtenerPorIdAsync(id);
        }
        else
        {
            // El médico solamente puede modificar sus propios turnos.
            turno = await _turnoService
                .ObtenerPorIdYMedicoUsuarioAsync(id, usuarioId);
        }

        if (turno == null)
            return NotFound(new { mensaje = "Turno no encontrado." });

        // El médico no puede cambiar el paciente ni el médico asignado.
        if (rol == nameof(RolUsuario.Medico) &&
            (datos.PacienteId != turno.PacienteId ||
             datos.MedicoId != turno.MedicoId))
        {
            return Forbid();
        }

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

    // DELETE: api/turnos/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Medico,Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuarioId = _usuarioActualService.ObtenerIdUsuario();
        var rol = _usuarioActualService.ObtenerRol();

        Turno? turno;

        if (rol == nameof(RolUsuario.Administrador))
        {
            turno = await _turnoService.ObtenerPorIdAsync(id);
        }
        else
        {
            // El médico solamente puede eliminar sus propios turnos.
            turno = await _turnoService
                .ObtenerPorIdYMedicoUsuarioAsync(id, usuarioId);
        }

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