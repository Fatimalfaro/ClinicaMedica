using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class PedidoLaboratorioRepository : IPedidoLaboratorioRepository
{
    private readonly ClinicaMedicaContext _context;

    public PedidoLaboratorioRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<PedidoLaboratorio>> ObtenerTodosAsync()
    {
        return await _context.PedidosLaboratorio
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PedidoLaboratorio?> ObtenerPorIdAsync(int id)
    {
        return await _context.PedidosLaboratorio
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AgregarAsync(PedidoLaboratorio pedido)
    {
        _context.PedidosLaboratorio.Add(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(PedidoLaboratorio pedido)
    {
        _context.PedidosLaboratorio.Update(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(PedidoLaboratorio pedido)
    {
        _context.PedidosLaboratorio.Remove(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return await _context.HistoriasClinicas
            .AnyAsync(h => h.Id == historiaClinicaId);
    }
}