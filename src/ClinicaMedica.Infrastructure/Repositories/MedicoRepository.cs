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
        var medicos = await _context.Medicos
            .AsNoTracking()
            .Include(m => m.Especialidad)
            .ToListAsync();

        foreach (var medico in medicos)
        {
            if (medico.Especialidad != null)
            {
                medico.Especialidad.Medicos = new List<Medico>();
            }
        }

        return medicos;
    }

    public async Task<Medico?> ObtenerPorIdAsync(int id)
    {
        var medico = await _context.Medicos
            .AsNoTracking()
            .Include(m => m.Especialidad)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medico?.Especialidad != null)
        {
            medico.Especialidad.Medicos = new List<Medico>();
        }

        return medico;
    }

    public async Task AgregarAsync(Medico medico)
    {
        await _context.Medicos.AddAsync(medico);
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
