using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class RecetaService
{
    private readonly IRecetaRepository _repository;

    public RecetaService(IRecetaRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Receta>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<Receta?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Receta receta)
    {
        return _repository.AgregarAsync(receta);
    }

    public Task ActualizarAsync(Receta receta)
    {
        return _repository.ActualizarAsync(receta);
    }

    public Task EliminarAsync(Receta receta)
    {
        return _repository.EliminarAsync(receta);
    }

    public Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return _repository.ExisteHistoriaClinicaAsync(historiaClinicaId);
    }
}