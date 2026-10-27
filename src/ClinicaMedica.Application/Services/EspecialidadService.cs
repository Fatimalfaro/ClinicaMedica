
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class EspecialidadService
{
    private readonly IEspecialidadRepository _repository;

    public EspecialidadService(IEspecialidadRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Especialidad>> ObtenerTodasAsync()
    {
        return _repository.ObtenerTodasAsync();
    }

    public Task<Especialidad?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Especialidad especialidad)
    {
        return _repository.AgregarAsync(especialidad);
    }

    public Task ActualizarAsync(Especialidad especialidad)
    {
        return _repository.ActualizarAsync(especialidad);
    }

    public Task EliminarAsync(Especialidad especialidad)
    {
        return _repository.EliminarAsync(especialidad);
    }
}
