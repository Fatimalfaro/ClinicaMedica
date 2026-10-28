using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IEspecialidadRepository
{
    Task<List<Especialidad>> ObtenerTodasAsync();

    Task<Especialidad?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Especialidad especialidad);

    Task ActualizarAsync(Especialidad especialidad);

    Task EliminarAsync(Especialidad especialidad);
}
