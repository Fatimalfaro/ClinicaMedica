
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicaMedica.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ClinicaMedicaContext _context;

    public UsuarioRepository(ClinicaMedicaContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExisteEmailAsync(string email)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExisteDniPacienteAsync(string dni)
    {
        return await _context.Pacientes
            .AnyAsync(p => p.DNI == dni);
    }

    public async Task RegistrarPacienteAsync(
        Usuario usuario,
        Paciente paciente)
    {
        paciente.Usuario = usuario;

        await _context.Usuarios.AddAsync(usuario);
        await _context.Pacientes.AddAsync(paciente);

        await _context.SaveChangesAsync();
    }
}
