
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ObtenerTodos()
    {
        var medicos = await _service.ObtenerTodosAsync();
        return Ok(medicos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var medico = await _service.ObtenerPorIdAsync(id);

        if (medico == null)
            return NotFound(new { mensaje = "Medico no encontrado" });

        return Ok(medico);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Medico medico)
    {
        await _service.AgregarAsync(medico);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = medico.Id },
            medico);
    }

    [HttpPut("{id:int}")]
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

        return Ok(medico);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var medico = await _service.ObtenerPorIdAsync(id);

        if (medico == null)
            return NotFound(new { mensaje = "Medico no encontrado" });

        await _service.EliminarAsync(medico);

        return NoContent();
    }
}
