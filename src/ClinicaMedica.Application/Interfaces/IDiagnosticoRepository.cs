using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IDiagnosticoRepository
{
    Task<List<Diagnostico>> ObtenerTodosAsync();

    Task<Diagnostico?> ObtenerPorIdAsync(int id);

    Task<List<Diagnostico>> ObtenerPorHistoriaClinicaAsync(
        int historiaClinicaId);

    Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId);

    Task AgregarAsync(Diagnostico diagnostico);
}