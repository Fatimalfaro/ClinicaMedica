using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Route("api/diagnosticos")]
public class DiagnosticosController : ControllerBase
{
    private readonly DiagnosticoService _service;

    public DiagnosticosController(DiagnosticoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var diagnosticos = await _service.ObtenerTodosAsync();

        var resultado = diagnosticos.Select(d => new DiagnosticoDto
        {
            Id = d.Id,
            HistoriaClinicaId = d.HistoriaClinicaId,
            Nombre = d.Nombre,
            Descripcion = d.Descripcion,
            FechaDiagnostico = d.FechaDiagnostico
        });

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var diagnostico = await _service.ObtenerPorIdAsync(id);

        if (diagnostico == null)
        {
            return NotFound(new
            {
                mensaje = "No se encontró el diagnóstico."
            });
        }

        var resultado = new DiagnosticoDto
        {
            Id = diagnostico.Id,
            HistoriaClinicaId = diagnostico.HistoriaClinicaId,
            Nombre = diagnostico.Nombre,
            Descripcion = diagnostico.Descripcion,
            FechaDiagnostico = diagnostico.FechaDiagnostico
        };

        return Ok(resultado);
    }

    [HttpGet("historia/{historiaClinicaId:int}")]
    public async Task<IActionResult> ObtenerPorHistoriaClinica(
        int historiaClinicaId)
    {
        var existeHistoria = await _service.ExisteHistoriaClinicaAsync(
            historiaClinicaId);

        if (!existeHistoria)
        {
            return NotFound(new
            {
                mensaje = "No se encontró la historia clínica."
            });
        }

        var diagnosticos = await _service.ObtenerPorHistoriaClinicaAsync(
            historiaClinicaId);

        var resultado = diagnosticos.Select(d => new DiagnosticoDto
        {
            Id = d.Id,
            HistoriaClinicaId = d.HistoriaClinicaId,
            Nombre = d.Nombre,
            Descripcion = d.Descripcion,
            FechaDiagnostico = d.FechaDiagnostico
        });

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearDiagnosticoDto dto)
    {
        var existeHistoria = await _service.ExisteHistoriaClinicaAsync(
            dto.HistoriaClinicaId);

        if (!existeHistoria)
        {
            return BadRequest(new
            {
                mensaje = "La historia clínica indicada no existe."
            });
        }

        var diagnostico = new Diagnostico
        {
            HistoriaClinicaId = dto.HistoriaClinicaId,
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            FechaDiagnostico = DateTime.Now
        };

        await _service.AgregarAsync(diagnostico);

        var resultado = new DiagnosticoDto
        {
            Id = diagnostico.Id,
            HistoriaClinicaId = diagnostico.HistoriaClinicaId,
            Nombre = diagnostico.Nombre,
            Descripcion = diagnostico.Descripcion,
            FechaDiagnostico = diagnostico.FechaDiagnostico
        };

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = diagnostico.Id },
            resultado);
    }
}