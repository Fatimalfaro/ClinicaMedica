using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/medicos")]
public class MedicosController : ControllerBase
{
    private readonly MedicoService _service;

    public MedicosController(MedicoService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodos()
    {
        var medicos = await _service.ObtenerTodosAsync();
        var resultado = medicos.Select(MapearMedico).ToList();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var medico = await _service.ObtenerPorIdAsync(id);

        if (medico == null)
            return NotFound(new { mensaje = "Medico no encontrado" });

        return Ok(MapearMedico(medico));
    }

    [HttpPost]
    [Authorize(Policy = "SoloAdministrador")]
    public async Task<IActionResult> Crear(Medico medico)
    {
        await _service.AgregarAsync(medico);

        var medicoCreado = await _service.ObtenerPorIdAsync(medico.Id);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = medico.Id },
            MapearMedico(medicoCreado ?? medico));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "SoloAdministrador")]
    public async Task<IActionResult> Actualizar(int id, Medico datos)
    {
        var medico = await _service.ObtenerPorIdAsync(id);

        if (medico == null)
            return NotFound(new { mensaje = "Medico no encontrado" });

        medico.Nombre = datos.Nombre;
        medico.Apellido = datos.Apellido;
        medico.Matricula = datos.Matricula;
        medico.Email = datos.Email;
        medico.Telefono = datos.Telefono;
        medico.PrecioConsulta = datos.PrecioConsulta;
        medico.EspecialidadId = datos.EspecialidadId;

        await _service.ActualizarAsync(medico);

        var medicoActualizado = await _service.ObtenerPorIdAsync(id);

        return Ok(MapearMedico(medicoActualizado ?? medico));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "SoloAdministrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var medico = await _service.ObtenerPorIdAsync(id);

        if (medico == null)
            return NotFound(new { mensaje = "Medico no encontrado" });

        await _service.EliminarAsync(medico);

        return NoContent();
    }

    private static MedicoDto MapearMedico(Medico medico)
    {
        return new MedicoDto
        {
            Id = medico.Id,
            Nombre = medico.Nombre,
            Apellido = medico.Apellido,
            Matricula = medico.Matricula,
            Email = medico.Email,
            Telefono = medico.Telefono,
            PrecioConsulta = medico.PrecioConsulta,
            Especialidad = medico.Especialidad == null
                ? null
                : new EspecialidadResumenDto
                {
                    Id = medico.Especialidad.Id,
                    Nombre = medico.Especialidad.Nombre,
                    Descripcion = medico.Especialidad.Descripcion
                }
        };
    }
}

