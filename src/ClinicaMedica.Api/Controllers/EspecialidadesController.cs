using ClinicaMedica.Application.DTOs;
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
        var resultado = especialidades.Select(e => new EspecialidadDto
        {
            Id = e.Id,
            Nombre = e.Nombre,
            Descripcion = e.Descripcion,
            Medicos = e.Medicos.Select(m => new MedicoResumenDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellido = m.Apellido,
                Matricula = m.Matricula,
                Email = m.Email,
                Telefono = m.Telefono,
                PrecioConsulta = m.PrecioConsulta
            }).ToList()
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var especialidad = await _service.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return NotFound(new { mensaje = "Especialidad no encontrada" });

        var resultado = new EspecialidadSimpleDto
        {
            Id = especialidad.Id,
            Nombre = especialidad.Nombre,
            Descripcion = especialidad.Descripcion
        };

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Especialidad especialidad)
    {
        await _service.AgregarAsync(especialidad);

        var resultado = new EspecialidadSimpleDto
        {
            Id = especialidad.Id,
            Nombre = especialidad.Nombre,
            Descripcion = especialidad.Descripcion
        };

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = especialidad.Id },
            resultado);
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

         var resultado = new EspecialidadSimpleDto
        {
            Id = especialidad.Id,
            Nombre = especialidad.Nombre,
            Descripcion = especialidad.Descripcion
        };

        return Ok(resultado);
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
