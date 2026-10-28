using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IEvolucionRepository
{
    Task<List<Evolucion>> ObtenerTodosAsync();
    Task<Evolucion?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Evolucion evolucion);
    Task ActualizarAsync(Evolucion evolucion);
    Task EliminarAsync(Evolucion evolucion);

    Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId);
}