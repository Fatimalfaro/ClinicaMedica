using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class RecetaRepository : IRecetaRepository
{
    private readonly ClinicaMedicaContext _context;

    public RecetaRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Receta>> ObtenerTodosAsync()
    {
        return await _context.Recetas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Receta?> ObtenerPorIdAsync(int id)
    {
        return await _context.Recetas
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AgregarAsync(Receta receta)
    {
        await _context.Recetas.AddAsync(receta);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Receta receta)
    {
        _context.Recetas.Update(receta);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Receta receta)
    {
        _context.Recetas.Remove(receta);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return await _context.HistoriasClinicas
            .AnyAsync(h => h.Id == historiaClinicaId);
    }
}