using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IPagoRepository
{
    Task<List<Pago>> ObtenerTodosAsync();
    Task<Pago?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Pago pago);
    Task<bool> ExisteTurnoAsync(int turnoId);
    Task<decimal?> ObtenerMontoTurnoAsync(int turnoId);
}