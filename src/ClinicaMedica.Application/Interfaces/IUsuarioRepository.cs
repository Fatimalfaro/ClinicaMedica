using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Usuario usuario);
}