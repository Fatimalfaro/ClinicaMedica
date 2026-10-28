using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class EvolucionService
{
    private readonly IEvolucionRepository _repository;

    public EvolucionService(IEvolucionRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Evolucion>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<Evolucion?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Evolucion evolucion)
    {
        return _repository.AgregarAsync(evolucion);
    }

    public Task ActualizarAsync(Evolucion evolucion)
    {
        return _repository.ActualizarAsync(evolucion);
    }

    public Task EliminarAsync(Evolucion evolucion)
    {
        return _repository.EliminarAsync(evolucion);
    }

    public Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return _repository.ExisteHistoriaClinicaAsync(historiaClinicaId);
    }
}