using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly ClinicaMedicaContext _context;

    public PacienteRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<Paciente>> ObtenerTodosAsync()
    {
        return await _context.Pacientes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Paciente?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AgregarAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Paciente paciente)
    {
        _context.Pacientes.Update(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Paciente paciente)
    {
        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();
    }
}
