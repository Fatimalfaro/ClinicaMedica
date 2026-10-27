
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class EspecialidadRepository : IEspecialidadRepository
{
    private readonly ClinicaMedicaContext _context;

    public EspecialidadRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Especialidad>> ObtenerTodasAsync()
    {
        return await _context.Especialidades
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Especialidad?> ObtenerPorIdAsync(int id)
    {
        return await _context.Especialidades
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AgregarAsync(Especialidad especialidad)
    {
        _context.Especialidades.Add(especialidad);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Especialidad especialidad)
    {
        _context.Especialidades.Update(especialidad);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Especialidad especialidad)
    {
        _context.Especialidades.Remove(especialidad);
        await _context.SaveChangesAsync();
    }
}
