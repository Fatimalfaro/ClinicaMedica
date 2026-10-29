using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class PagoService
{
    private readonly IPagoRepository _repository;

    public PagoService(IPagoRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Pago>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<Pago?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Pago pago)
    {
        return _repository.AgregarAsync(pago);
    }

    public Task<bool> ExisteTurnoAsync(int turnoId)
    {
        return _repository.ExisteTurnoAsync(turnoId);
    }

    public Task<decimal?> ObtenerMontoTurnoAsync(int turnoId)
    {
        return _repository.ObtenerMontoTurnoAsync(turnoId);
    }
}