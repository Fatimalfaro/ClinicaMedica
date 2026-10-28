using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class HistoriaClinicaRepository : IHistoriaClinicaRepository
{
    private readonly ClinicaMedicaContext _context;

    public HistoriaClinicaRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<List<HistoriaClinica>> ObtenerTodosAsync()
    {
        return await _context.HistoriasClinicas
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<HistoriaClinica?> ObtenerPorIdAsync(int id)
    {
        return await _context.HistoriasClinicas
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task AgregarAsync(HistoriaClinica historiaClinica)
    {
        await _context.HistoriasClinicas.AddAsync(historiaClinica);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(HistoriaClinica historiaClinica)
    {
        _context.HistoriasClinicas.Update(historiaClinica);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(HistoriaClinica historiaClinica)
    {
        _context.HistoriasClinicas.Remove(historiaClinica);
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