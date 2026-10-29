using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class PagoRepository : IPagoRepository
{
    private readonly ClinicaMedicaContext _context;

    public PagoRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Pago>> ObtenerTodosAsync()
    {
        return await _context.Pagos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Pago?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pagos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AgregarAsync(Pago pago)
    {
        _context.Pagos.Add(pago);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteTurnoAsync(int turnoId)
    {
        return await _context.Turnos
            .AnyAsync(t => t.Id == turnoId);
    }

    public async Task<decimal?> ObtenerMontoTurnoAsync(int turnoId)
    {
        return await _context.Turnos
            .Where(t => t.Id == turnoId)
            .Select(t => (decimal?)t.Medico!.PrecioConsulta)
            .FirstOrDefaultAsync();
    }
}