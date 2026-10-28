using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class PacienteService
{
    private readonly IPacienteRepository _repository;

    public PacienteService(IPacienteRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Paciente>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<Paciente?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Paciente paciente)
    {
        return _repository.AgregarAsync(paciente);
    }

    public Task ActualizarAsync(Paciente paciente)
    {
        return _repository.ActualizarAsync(paciente);
    }

    public Task EliminarAsync(Paciente paciente)
    {
        return _repository.EliminarAsync(paciente);
    }
}
