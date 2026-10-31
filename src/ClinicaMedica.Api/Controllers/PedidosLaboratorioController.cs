using ClinicaMedica.Application.DTOs;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[Authorize(Policy = "MedicoOAdministrador")]

[Route("api/[controller]")]
public class PedidosLaboratorioController : ControllerBase
{
    private readonly PedidoLaboratorioService _service;

    public PedidosLaboratorioController(PedidoLaboratorioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var pedidos = await _service.ObtenerTodosAsync();

        var resultado = pedidos.Select(p => new
        {
            p.Id,
            p.HistoriaClinicaId,
            p.FechaSolicitud,
            p.EstudiosSolicitados,
            Estado = p.Estado.ToString()
        });

        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var pedido = await _service.ObtenerPorIdAsync(id);

        if (pedido == null)
            return NotFound(new { mensaje = "Pedido de laboratorio no encontrado." });

        return Ok(new
        {
            pedido.Id,
            pedido.HistoriaClinicaId,
            pedido.FechaSolicitud,
            pedido.EstudiosSolicitados,
            Estado = pedido.Estado.ToString()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        CrearPedidoLaboratorioDto dto)
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

        var pedido = new PedidoLaboratorio
        {
            HistoriaClinicaId = dto.HistoriaClinicaId,
            FechaSolicitud = DateTime.Now,
            EstudiosSolicitados = dto.EstudiosSolicitados
        };

        await _service.AgregarAsync(pedido);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = pedido.Id },
            new
            {
                pedido.Id,
                pedido.HistoriaClinicaId,
                pedido.FechaSolicitud,
                pedido.EstudiosSolicitados,
                Estado = pedido.Estado.ToString()
            });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarPedidoLaboratorioDto dto)
    {
        var pedido = await _service.ObtenerPorIdAsync(id);

        if (pedido == null)
            return NotFound(new { mensaje = "Pedido de laboratorio no encontrado." });

        pedido.EstudiosSolicitados = dto.EstudiosSolicitados;

        await _service.ActualizarAsync(pedido);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var pedido = await _service.ObtenerPorIdAsync(id);

        if (pedido == null)
            return NotFound(new { mensaje = "Pedido de laboratorio no encontrado." });

        await _service.EliminarAsync(pedido);

        return NoContent();
    }
}