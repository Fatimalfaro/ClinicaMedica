
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class MedicoRepository : IMedicoRepository
{
    private readonly ClinicaMedicaContext _context;

    public MedicoRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Medico>> ObtenerTodosAsync()
    {
        return await _context.Medicos
            .AsNoTracking()
            .Include(m => m.Especialidad)
            .ToListAsync();
    }

    public async Task<Medico?> ObtenerPorIdAsync(int id)
    {
        return await _context.Medicos
            .AsNoTracking()
            .Include(m => m.Especialidad)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AgregarAsync(Medico medico)
    {
        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Medico medico)
    {
        _context.Medicos.Update(medico);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Medico medico)
    {
        _context.Medicos.Remove(medico);
        await _context.SaveChangesAsync();
    }
}
