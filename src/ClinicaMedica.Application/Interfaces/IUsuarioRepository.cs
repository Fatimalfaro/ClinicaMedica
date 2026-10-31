using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteEmailAsync(string email);

    Task<bool> ExisteDniPacienteAsync(string dni);

    Task RegistrarPacienteAsync(Usuario usuario, Paciente paciente);
}