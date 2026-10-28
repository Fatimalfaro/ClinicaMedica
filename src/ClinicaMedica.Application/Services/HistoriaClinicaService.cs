using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class HistoriaClinicaService
{
    private readonly IHistoriaClinicaRepository _repository;

    public HistoriaClinicaService(IHistoriaClinicaRepository repository)
    {
        _repository = repository;
    }

    public Task<List<HistoriaClinica>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<HistoriaClinica?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(HistoriaClinica historiaClinica)
    {
        return _repository.AgregarAsync(historiaClinica);
    }

    public Task ActualizarAsync(HistoriaClinica historiaClinica)
    {
        return _repository.ActualizarAsync(historiaClinica);
    }

    public Task EliminarAsync(HistoriaClinica historiaClinica)
    {
        return _repository.EliminarAsync(historiaClinica);
    }

    public Task<bool> ExistePacienteAsync(int pacienteId)
    {
        return _repository.ExistePacienteAsync(pacienteId);
    }

    public Task<bool> ExisteMedicoAsync(int medicoId)
    {
        return _repository.ExisteMedicoAsync(medicoId);
    }
}