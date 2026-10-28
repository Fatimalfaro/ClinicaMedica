using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class EvolucionRepository : IEvolucionRepository
{
    private readonly ClinicaMedicaContext _context;

    public EvolucionRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Evolucion>> ObtenerTodosAsync()
    {
        return await _context.Evoluciones
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Evolucion?> ObtenerPorIdAsync(int id)
    {
        return await _context.Evoluciones
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AgregarAsync(Evolucion evolucion)
    {
        await _context.Evoluciones.AddAsync(evolucion);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Evolucion evolucion)
    {
        _context.Evoluciones.Update(evolucion);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Evolucion evolucion)
    {
        _context.Evoluciones.Remove(evolucion);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return await _context.HistoriasClinicas
            .AnyAsync(h => h.Id == historiaClinicaId);
    }
}