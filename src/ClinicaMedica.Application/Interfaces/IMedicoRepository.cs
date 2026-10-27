
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IMedicoRepository
{
    Task<List<Medico>> ObtenerTodosAsync();

    Task<Medico?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Medico medico);

    Task ActualizarAsync(Medico medico);

    Task EliminarAsync(Medico medico);
}
