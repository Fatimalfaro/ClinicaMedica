using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class DiagnosticoRepository : IDiagnosticoRepository
{
    private readonly ClinicaMedicaContext _context;

    public DiagnosticoRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Diagnostico>> ObtenerTodosAsync()
    {
        return await _context.Diagnosticos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Diagnostico?> ObtenerPorIdAsync(int id)
    {
        return await _context.Diagnosticos
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<List<Diagnostico>>
        ObtenerPorHistoriaClinicaAsync(int historiaClinicaId)
    {
        return await _context.Diagnosticos
            .AsNoTracking()
            .Where(d => d.HistoriaClinicaId == historiaClinicaId)
            .ToListAsync();
    }

    public async Task<bool> ExisteHistoriaClinicaAsync(
        int historiaClinicaId)
    {
        return await _context.HistoriasClinicas
            .AnyAsync(h => h.Id == historiaClinicaId);
    }

    public async Task AgregarAsync(Diagnostico diagnostico)
    {
        _context.Diagnosticos.Add(diagnostico);
        await _context.SaveChangesAsync();
    }
}