using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Authorize(Policy = "SoloAdministrador")]

[Route("api/pacientes")]
public class PacientesController : ControllerBase
{
    private readonly PacienteService _service;

    public PacientesController(PacienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var pacientes = await _service.ObtenerTodosAsync();

        var resultado = pacientes.Select(MapearPaciente).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var paciente = await _service.ObtenerPorIdAsync(id);

        if (paciente == null)
            return NotFound(new { mensaje = "Paciente no encontrado" });

        return Ok(MapearPaciente(paciente));
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearPacienteDto datos)
    {
        var paciente = new Paciente
        {
            Nombre = datos.Nombre,
            Apellido = datos.Apellido,
            DNI = datos.DNI,
            FechaNacimiento = datos.FechaNacimiento,
            Email = datos.Email,
            Telefono = datos.Telefono
        };

        await _service.AgregarAsync(paciente);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = paciente.Id },
            MapearPaciente(paciente));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarPacienteDto datos)
    {
        var paciente = await _service.ObtenerPorIdAsync(id);

        if (paciente == null)
            return NotFound(new { mensaje = "Paciente no encontrado" });

        paciente.Nombre = datos.Nombre;
        paciente.Apellido = datos.Apellido;
        paciente.DNI = datos.DNI;
        paciente.FechaNacimiento = datos.FechaNacimiento;
        paciente.Email = datos.Email;
        paciente.Telefono = datos.Telefono;

        await _service.ActualizarAsync(paciente);

        return Ok(MapearPaciente(paciente));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var paciente = await _service.ObtenerPorIdAsync(id);

        if (paciente == null)
            return NotFound(new { mensaje = "Paciente no encontrado" });

        await _service.EliminarAsync(paciente);

        return NoContent();
    }

    private static PacienteDto MapearPaciente(Paciente paciente)
    {
        return new PacienteDto
        {
            Id = paciente.Id,
            Nombre = paciente.Nombre,
            Apellido = paciente.Apellido,
            DNI = paciente.DNI,
            FechaNacimiento = paciente.FechaNacimiento,
            Email = paciente.Email,
            Telefono = paciente.Telefono
        };
    }
}
