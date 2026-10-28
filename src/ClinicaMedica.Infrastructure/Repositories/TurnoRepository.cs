using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class TurnoRepository : ITurnoRepository
{
    private readonly ClinicaMedicaContext _context;

    public TurnoRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Turno>> ObtenerTodosAsync()
    {
        return await _context.Turnos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Turno?> ObtenerPorIdAsync(int id)
    {
        return await _context.Turnos
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AgregarAsync(Turno turno)
    {
        await _context.Turnos.AddAsync(turno);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Turno turno)
    {
        _context.Turnos.Update(turno);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Turno turno)
    {
        _context.Turnos.Remove(turno);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistePacienteAsync(int pacienteId)
    {
        return await _context.Pacientes
            .AnyAsync(p => p.Id == pacienteId);
    }

    public async Task<bool> ExisteMedicoAsync(int medicoId)
    {
        return await _context.Medicos
            .AnyAsync(m => m.Id == medicoId);
    }
}