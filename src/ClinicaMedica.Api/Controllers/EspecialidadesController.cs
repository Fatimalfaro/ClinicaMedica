
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/especialidades")]
public class EspecialidadesController : ControllerBase
{
    private readonly EspecialidadService _service;

    public EspecialidadesController(EspecialidadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        var especialidades = await _service.ObtenerTodasAsync();
        return Ok(especialidades);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var especialidad = await _service.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return NotFound(new { mensaje = "Especialidad no encontrada" });

        return Ok(especialidad);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Especialidad especialidad)
    {
        await _service.AgregarAsync(especialidad);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = especialidad.Id },
            especialidad);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Especialidad datos)
    {
        var especialidad = await _service.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return NotFound(new { mensaje = "Especialidad no encontrada" });

        especialidad.Nombre = datos.Nombre;
        especialidad.Descripcion = datos.Descripcion;

        await _service.ActualizarAsync(especialidad);

        return Ok(especialidad);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var especialidad = await _service.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return NotFound(new { mensaje = "Especialidad no encontrada" });

        await _service.EliminarAsync(especialidad);

        return NoContent();
    }
}
